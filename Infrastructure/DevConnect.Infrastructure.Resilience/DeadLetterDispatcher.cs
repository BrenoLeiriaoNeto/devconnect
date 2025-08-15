using System.Text;
using System.Text.Json;
using DevConnect.Application.Contracts.Models.EmailModels;
using DevConnect.Infrastructure.Subscriber.Interfaces;
using Microsoft.Extensions.Logging;

namespace DevConnect.Infrastructure.Resilience;

public class DeadLetterDispatcher(IRabbitMqConnectionFactory connectionFactory, 
    ILogger<DeadLetterDispatcher> logger)
{
    public async Task PublishToDeadLetterQueueAsync(VerifyEmailModel model)
    {
        try
        {
            using var connection = await connectionFactory.CreateConnectionAsync();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "dead-letter-email",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        
            var payload = JsonSerializer.Serialize(model);
            var body = Encoding.UTF8.GetBytes(payload);
        
        
            var props = channel.CreateBasicProperties();
            props.ContentType = "application/json";
            props.DeliveryMode = 2;
        
            channel.BasicPublish(
                exchange: "",
                routingKey: "dead-letter-email",
                mandatory: false,
                basicProperties: props,
                body: body
            );
            logger.LogInformation("Published message to dead letter queue: {@VerifyEmailModel}", model);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to publish message to dead letter queue: {@VerifyEmailModel}", model);
            throw;
        }
    }
}