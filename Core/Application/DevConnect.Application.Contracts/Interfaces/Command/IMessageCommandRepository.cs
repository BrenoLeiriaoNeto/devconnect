using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Command;

public interface IMessageCommandRepository
{
    Task AddAsync(Message message);
}