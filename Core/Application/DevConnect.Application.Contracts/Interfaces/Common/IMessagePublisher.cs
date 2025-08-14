using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Common;

public interface IMessagePublisher
{
    Task PublishMessageAsync<T>(T message);
}