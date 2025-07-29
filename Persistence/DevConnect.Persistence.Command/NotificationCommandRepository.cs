using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;
using MongoDB.Driver.Linq;

namespace DevConnect.Persistence.Command;

public class NotificationCommandRepository : INotificationCommandRepository
{
    private readonly DevConnectMongoContext _mongoContext;

    public NotificationCommandRepository(DevConnectMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }
    
    public async Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification = await _mongoContext.Notifications.FirstOrDefaultAsync(
            n => n.Id == notificationId, cancellationToken);

        if (notification is not null && notification.IsRead == false)
        {
            notification.MarkAsRead();
            _mongoContext.Notifications.Update(notification);
            await _mongoContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task AddNotificationAsync(Notification notification, CancellationToken cancellationToken)
    {
        await _mongoContext.Notifications.AddAsync(notification, cancellationToken);
        await _mongoContext.SaveChangesAsync(cancellationToken);
    }
}