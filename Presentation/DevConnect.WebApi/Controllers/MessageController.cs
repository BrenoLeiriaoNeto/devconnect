using DevConnect.Application.Services.Commands;
using DevConnect.Application.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.WebApi.Controllers;

[ApiController]
[Route("api/messages")]
public class MessageController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SendMessage([FromBody] SendMessageCommand command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Created();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMessagesForUser(Guid id, CancellationToken cancellationToken)
    {
        var messages = new GetMessagesForUserQuery(id);
        var result = await mediator.Send(messages, cancellationToken);
        return Ok(result);
    }
}