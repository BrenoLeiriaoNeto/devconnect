using DevConnect.Application.Services.Commands;
using DevConnect.Application.Services.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.WebApi.Controllers;

[ApiController]
[Route("api/user-profile")]
public class UserProfileController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateUserProfile([FromBody] CreateUserProfileCommand command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Created();
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateUserProfile(Guid id, [FromBody] UpdateUserProfileCommand command,
        CancellationToken cancellationToken)
    {
        if (command.Id != id)
        {
            return BadRequest("Mismatched user ID between route and payload");
        }
        
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}