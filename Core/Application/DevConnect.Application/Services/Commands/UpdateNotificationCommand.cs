using MediatR;

namespace DevConnect.Application.Services.Commands;

public class UpdateNotificationCommand(Guid notificationId) : IRequest
{
    public Guid NotificationId { get; }
}