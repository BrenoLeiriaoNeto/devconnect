using System.Text;
using System.Text.Json;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Infrastructure.Publisher.Models;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DevConnect.Infrastructure.Publisher;

public class RabbitMqPublisher(IOptions<RabbitMqSettingsModel> options) : IMessagePublisher
{
    private readonly RabbitMqSettingsModel _settings = options.Value;

    public Task PublishMessageAsync<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(
            exchange: _settings.ExchangeName,
            type: ExchangeType.Direct,
            durable: true
        );

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2;

        channel.BasicPublish(
            exchange: _settings.ExchangeName,
            routingKey: _settings.RoutingKey,
            basicProperties: props,
            body: body
        );
        return Task.CompletedTask;
    }
}