using DevConnect.Infrastructure.Models;
using DevConnect.Infrastructure.Subscriber.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DevConnect.Infrastructure.Subscriber;

public class RabbitMqConnectionFactory(
    ILogger<RabbitMqConnectionFactory> logger,
    IOptions<RabbitMqSettingsModel> options)
    : IRabbitMqConnectionFactory
{
    private readonly RabbitMqSettingsModel _settings = options.Value;

    public Task<IConnection> CreateConnectionAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            DispatchConsumersAsync = true
        };
        try
        {
            var connection = factory.CreateConnection();
            logger.LogInformation("Connection to RabbitMq established: {Hostname}", _settings.HostName);
            return Task.FromResult(connection);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Connection to RabbitMq failed: {Hostname}", _settings.HostName);
            throw;
        }
    }
}