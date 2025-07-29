using DevConnect.Application.Contracts.Models.InputModels;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping.Interfaces;

public interface INotificationMapper
{
    Notification ToDomain(NotificationInputModel input);
    NotificationViewModel ToViewModel(Notification domain);
}