using DevConnect.Infrastructure.Models;
using DevConnect.Infrastructure.Subscriber.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevConnect.Infrastructure.Subscriber;

public class RabbitMqInitializer(
    ILogger<RabbitMqInitializer> logger,
    IOptions<RabbitMqSettingsModel> options,
    IRabbitMqConnectionFactory connectionFactory)
    : IHostedService
{
    private readonly RabbitMqSettingsModel _settings = options.Value;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var connection = await connectionFactory.CreateConnectionAsync();
        using var channel = connection.CreateModel();
        
        channel.ExchangeDeclare(
            exchange: _settings.ExchangeName,
            type: _settings.ExchangeType,
            durable: true,
            autoDelete: false,
            arguments: null
            );

        channel.QueueDeclare(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        channel.QueueBind(
            queue: _settings.QueueName,
            exchange: _settings.ExchangeName,
            routingKey: _settings.RoutingKey,
            arguments: null
            );
        
        logger.LogInformation("RabbitMq exchange and queue declared at startup");
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}