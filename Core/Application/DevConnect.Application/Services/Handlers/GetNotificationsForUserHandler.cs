using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Queries;
using DevConnect.Exceptions;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class GetNotificationsForUserHandler(INotificationQueryRepository notificationQueryRepository, 
    INotificationMapper notificationMapper) : IRequestHandler<GetNotificationsForUserQuery, IEnumerable<NotificationViewModel>>
{
    public async Task<IEnumerable<NotificationViewModel>> Handle(GetNotificationsForUserQuery request, CancellationToken cancellationToken)
    {
        var notifications = await notificationQueryRepository
            .GetNotificationsForUserAsync(request.UserId, cancellationToken);

        if (notifications is null || !notifications.Any())
        {
            throw new EntityNotFoundException($"Notifications for user with ID '{request.UserId}' not found.");
        }
        
        var notificationsViewModel = notifications.Select(notificationMapper.ToViewModel);
        
        return notificationsViewModel;
    }
}