using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Queries;
using DevConnect.Exceptions;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class GetMessagesForUserHandler(IMessageQueryRepository messageQueryRepository, 
    IMessageMapper messageMapper) : IRequestHandler<GetMessagesForUserQuery, IEnumerable<MessageViewModel>>
{
    public async Task<IEnumerable<MessageViewModel>> Handle(GetMessagesForUserQuery request,
        CancellationToken cancellationToken)
    {
        var messages = await messageQueryRepository.GetMessagesForUserAsync(
            request.UserId, cancellationToken);

        if (messages is null || !messages.Any())
        {
            throw new EntityNotFoundException($"Messages for user with ID '{request.UserId}' not found.");
        }
        
        var messagesViewModel = messages.Select(messageMapper.ToViewModel);
        
        return messagesViewModel;
    }
}