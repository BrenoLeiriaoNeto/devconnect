using DevConnect.Application.Contracts.Models.InputModels;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping;

public class MessageMapper : IMessageMapper
{
    public Message ToDomain(MessageInputModel input)
    {
        return new Message(input.SenderId, input.ReceiverId, input.Content);
    }

    public MessageViewModel ToViewModel(Message domain)
    {
        return new MessageViewModel
        {
            SenderId = domain.SenderId,
            ReceiverId = domain.ReceiverId,
            Content = domain.Content,
            SentAt = domain.SentAt,
            IsRead = domain.IsRead,
        };
    }
}