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
    
    public async Task MarkAsReadAsync(Guid notificationId)
    {
        var notification = await _mongoContext.Notifications.FirstOrDefaultAsync(
            n => n.Id == notificationId);

        if (notification is not null && notification.IsRead == false)
        {
            notification.MarkAsRead();
            _mongoContext.Notifications.Update(notification);
            await _mongoContext.SaveChangesAsync();
        }
    }

    public async Task AddNotificationAsync(Notification notification)
    {
        await _mongoContext.Notifications.AddAsync(notification);
        await _mongoContext.SaveChangesAsync();
    }
}