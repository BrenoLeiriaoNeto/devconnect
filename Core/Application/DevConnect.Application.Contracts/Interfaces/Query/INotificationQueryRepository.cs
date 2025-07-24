using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Query;

public interface INotificationQueryRepository
{
    Task<IEnumerable<Notification>> GetNotificationsForUserAsync(Guid userId);
}