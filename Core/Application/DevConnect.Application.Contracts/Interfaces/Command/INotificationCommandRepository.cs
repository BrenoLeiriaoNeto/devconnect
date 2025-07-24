using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Command;

public interface INotificationCommandRepository
{
    Task MarkAsReadAsync(Guid notificationId);
    Task AddNotificationAsync(Notification notification);
}