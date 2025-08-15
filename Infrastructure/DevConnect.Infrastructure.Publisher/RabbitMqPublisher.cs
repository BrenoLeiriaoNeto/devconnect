using System.Text;
using System.Text.Json;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Application.Contracts.Models.EmailModels;
using DevConnect.Infrastructure.Models;
using DevConnect.Infrastructure.Resilience;
using DevConnect.Infrastructure.Subscriber.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DevConnect.Infrastructure.Publisher;

public class RabbitMqPublisher: IMessagePublisher, IDisposable
{
    private readonly RabbitMqSettingsModel _settings;
    private readonly ILogger<RabbitMqPublisher> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly DeadLetterDispatcher _deadLetterDispatcher;

    public RabbitMqPublisher(
        IOptions<RabbitMqSettingsModel> options,
        IRabbitMqConnectionFactory connectionFactory,
        ILogger<RabbitMqPublisher> logger,
        DeadLetterDispatcher deadLetterDispatcher)
    {
        _settings = options.Value;
        _logger = logger;
        _connection = connectionFactory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateModel();
        _deadLetterDispatcher = deadLetterDispatcher;
    }
    
    public async Task PublishMessageAsync<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = _channel.CreateBasicProperties();
        props.ContentType = "application/json";
        props.DeliveryMode = 2;

        var retryPolicy = RetryPolicies.GetRabbitMqRetryPolicy();

        try
        {
            await retryPolicy.ExecuteAsync(() =>
            {
                _channel.BasicPublish(
                    exchange: _settings.ExchangeName,
                    routingKey: _settings.RoutingKey,
                    basicProperties: props,
                    body: body
                );
                _logger.LogInformation("Publishing message to exchange {Exchange} with routing key {RoutingKey}",
                    _settings.ExchangeName, _settings.RoutingKey);

                return Task.CompletedTask;
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to publish message to RabbitMQ. Sending to dead letter queue...");

            if (message is VerifyEmailModel emailModel)
            {
                await _deadLetterDispatcher.PublishToDeadLetterQueueAsync(emailModel);
            }
            else
            {
                _logger.LogWarning("Message type {Type} not supported by dead letter dispatcher", typeof(T).Name);
            }
        }
    }

    public void Dispose()
    {
        _channel.Close();
        _channel.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}