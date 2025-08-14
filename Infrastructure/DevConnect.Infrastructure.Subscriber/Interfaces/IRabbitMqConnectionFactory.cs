using RabbitMQ.Client;

namespace DevConnect.Infrastructure.Subscriber.Interfaces;

public interface IRabbitMqConnectionFactory
{
    Task<IConnection> CreateConnectionAsync();
}