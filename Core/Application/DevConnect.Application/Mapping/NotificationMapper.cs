using DevConnect.Application.Contracts.Models.InputModels;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping;

public class NotificationMapper : INotificationMapper
{
    public Notification ToDomain(NotificationInputModel input)
    {
        return new Notification(input.UserId, input.Title, input.Message);
    }

    public NotificationViewModel ToViewModel(Notification domain)
    {
        return new NotificationViewModel
        {
            UserId = domain.UserId,
            Title = domain.Title,
            Message = domain.Message,
            IsRead = domain.IsRead,
        };
    }
}