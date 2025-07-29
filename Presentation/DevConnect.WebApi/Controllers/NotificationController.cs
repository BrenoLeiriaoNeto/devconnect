using DevConnect.Application.Services.Commands;
using DevConnect.Application.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.WebApi.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> InsertNotification([FromBody] SendNotificationCommand command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateNotification(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetNotificationsForUser(Guid id, CancellationToken cancellationToken)
    {
        var notifications = new GetNotificationsForUserQuery(id);
        var result = await mediator.Send(notifications, cancellationToken);
        return Ok(result);       
    }
}