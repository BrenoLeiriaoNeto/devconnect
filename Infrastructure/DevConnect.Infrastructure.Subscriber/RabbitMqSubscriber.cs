using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Infrastructure.Subscriber.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DevConnect.Infrastructure.Subscriber;

public class RabbitMqSubscriber : BackgroundService
{
    private readonly ILogger<RabbitMqSubscriber> _logger;
    private readonly IRabbitMqConnectionFactory _connectionFactory;
    private readonly IMessageHandler _messageHandler;
    private readonly IConfiguration _configuration;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;

    public RabbitMqSubscriber(
        ILogger<RabbitMqSubscriber> logger,
        IRabbitMqConnectionFactory connectionFactory,
        IMessageHandler messageHandler,
        IConfiguration configuration)
    {
        _logger = logger;
        _connectionFactory = connectionFactory;
        _messageHandler = messageHandler;
        _configuration = configuration;
    }

    private async Task InitializeRabbitMqAsync()
    {
        _queueName = _configuration["RabbitMQ:QueueName"];
        _connection = await _connectionFactory.CreateConnectionAsync();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
        
        _logger.LogInformation("RabbitMqSubscriber initialized");
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
                queue: _queueName,
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