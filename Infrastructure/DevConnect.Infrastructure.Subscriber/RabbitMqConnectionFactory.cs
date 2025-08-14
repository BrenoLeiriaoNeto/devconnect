using DevConnect.Infrastructure.Subscriber.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace DevConnect.Infrastructure.Subscriber;

public class RabbitMqConnectionFactory(IConfiguration configuration, 
    ILogger<RabbitMqConnectionFactory> logger) : IRabbitMqConnectionFactory
{
    public Task<IConnection> CreateConnectionAsync()
    {
        var hostname = configuration["RabbitMQ:HostName"];
        var port = int.Parse(configuration["RabbitMQ:Port"]);
        var username = configuration["RabbitMQ:UserName"];
        var password = configuration["RabbitMQ:Password"];
        var factory = new ConnectionFactory
        {
            HostName = hostname,
            Port = port,
            UserName = username,
            Password = password,
            DispatchConsumersAsync = true
        };
        try
        {
            var connection = factory.CreateConnection();
            logger.LogInformation("Connection to RabbitMq established: {Hostname}", hostname);
            return Task.FromResult(connection);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Connection to RabbitMq failed: {Hostname}", hostname);
            throw;
        }
    }
}