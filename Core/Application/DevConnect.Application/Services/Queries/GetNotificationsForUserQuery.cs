using DevConnect.Application.Contracts.Models.ViewModels;
using MediatR;

namespace DevConnect.Application.Services.Queries;

public class GetNotificationsForUserQuery(Guid userId) : IRequest<IEnumerable<NotificationViewModel>>
{
    public Guid UserId { get; }
}