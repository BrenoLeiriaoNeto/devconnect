using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Commands;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class SendNotificationHandler(INotificationCommandRepository notificationCommandRepository,
    INotificationMapper mapper) : IRequestHandler<SendNotificationCommand>
{
    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = mapper.ToDomain(request.Input);
        notification.CreatedAt = DateTime.UtcNow;
        notification.CreatedBy = "Test User";
        await notificationCommandRepository.AddNotificationAsync(notification, cancellationToken);
    }
}