using DevConnect.Application.Contracts.Models.ViewModels;
using MediatR;

namespace DevConnect.Application.Services.Queries;

public class GetMessagesForUserQuery(Guid userId) : IRequest<IEnumerable<MessageViewModel>>
{
    public Guid UserId { get; }
}