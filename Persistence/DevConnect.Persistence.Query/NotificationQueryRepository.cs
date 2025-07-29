using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;
using MongoDB.Driver.Linq;

namespace DevConnect.Persistence.Query;

public class NotificationQueryRepository : INotificationQueryRepository
{
    private readonly DevConnectMongoContext _mongoContext;

    public NotificationQueryRepository(DevConnectMongoContext context)
    {
        _mongoContext = context;
    }
    public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        IQueryable<Notification> query = _mongoContext.Notifications
            .Where(n => n.UserId == userId);

        var notifications = await query
            .OrderBy(n => n.CreatedAt)
            .ToListAsync(cancellationToken);

        return notifications;
    }

    public async Task<bool> HasReadNotificationAsync(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification = await _mongoContext.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId, cancellationToken);

        return notification.IsRead;
    }
}