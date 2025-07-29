using DevConnect.Application.Contracts.Models.InputModels;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping.Interfaces;

public interface IMessageMapper
{
    Message ToDomain(MessageInputModel input);
    MessageViewModel ToViewModel(Message domain);
}