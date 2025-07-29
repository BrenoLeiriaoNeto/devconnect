using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Services.Commands;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class UpdateNotificationHandler(INotificationCommandRepository notificationCommandRepository, 
    INotificationQueryRepository notificationQueryRepository)
    : IRequestHandler<UpdateNotificationCommand>
{
    public async Task Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var isRead = await notificationQueryRepository
            .HasReadNotificationAsync(request.NotificationId, cancellationToken);

        if (!isRead)
        {
            await notificationCommandRepository.MarkAsReadAsync(request.NotificationId, cancellationToken);
        }
    }
}