using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Infrastructure.Models;
using DevConnect.Infrastructure.Subscriber.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace DevConnect.Infrastructure.Subscriber;

public class RabbitMqSubscriber : BackgroundService
{
    private readonly ILogger<RabbitMqSubscriber> _logger;
    private readonly IRabbitMqConnectionFactory _connectionFactory;
    private readonly RabbitMqSettingsModel _settings;
    private readonly IMessageHandler _messageHandler;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMqSubscriber(
        ILogger<RabbitMqSubscriber> logger,
        IRabbitMqConnectionFactory connectionFactory,
        IOptions<RabbitMqSettingsModel> options,
        IMessageHandler messageHandler)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
        _settings = options.Value;
        _messageHandler = messageHandler;
    }

    private async Task InitializeRabbitMqAsync()
    {
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = _connection.CreateModel();
        
        _channel.ExchangeDeclare(
            _settings.ExchangeName,
            _settings.ExchangeType, 
            durable: true);

        _channel.QueueDeclare(
            queue: _settings.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        _channel.QueueBind(
            queue: _settings.QueueName,
            exchange: _settings.ExchangeName,
            routingKey: _settings.RoutingKey
            );
        
        _logger.LogInformation("RabbitMqSubscriber initialized and bound to exchange {Exchange}",
            _settings.ExchangeName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await InitializeRabbitMqAsync();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await _messageHandler.HandleMessageAsync(message);
            };

            _channel.BasicConsume(
                queue: _settings.QueueName,
                autoAck: true,
                consumer: consumer
            );
        
            _logger.LogInformation("Started consuming messages from RabbitMq");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "RabbitMqSubscriber failed to start");
        }
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection.Close();
        base.Dispose();
    }
}