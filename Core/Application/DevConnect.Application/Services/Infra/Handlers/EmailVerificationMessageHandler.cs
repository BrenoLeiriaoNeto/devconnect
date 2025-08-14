using System.Text.Json;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Application.Contracts.Models.EmailModels;
using DevConnect.Application.Services.Infra.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevConnect.Application.Services.Infra.Handlers;

public class EmailVerificationMessageHandler(
    IMediator mediator,
    ILogger<EmailVerificationMessageHandler> logger
    ) : IMessageHandler
{
    public async Task HandleMessageAsync(string message)
    {
        var verification = JsonSerializer.Deserialize<EmailVerificationMessageModel>(message);

        if (verification is null)
        {
            logger.LogWarning("Received null or invalid message");
            return;
        }

        var command = new EmailVerificationCommand(verification.VerificationToken);
        await mediator.Send(command);
    }
}