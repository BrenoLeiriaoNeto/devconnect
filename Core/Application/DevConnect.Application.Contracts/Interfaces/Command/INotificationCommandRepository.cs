using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Command;

public interface INotificationCommandRepository
{
    Task MarkAsReadAsync(Guid notificationId);
    Task AddAsync(Notification notification);
}