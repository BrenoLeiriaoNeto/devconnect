using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Commands;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class SendMessageHandler(IMessageCommandRepository messageCommandRepository, 
    IMessageMapper mapper) : IRequestHandler<SendMessageCommand>
{
    
    public async Task Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = mapper.ToDomain(request.Input);
        message.CreatedAt = DateTime.UtcNow;
        message.CreatedBy = "Test User";
        await messageCommandRepository.AddMessageAsync(message, cancellationToken);
    }
}