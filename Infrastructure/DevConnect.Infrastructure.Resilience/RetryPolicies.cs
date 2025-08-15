using MailKit.Net.Smtp;
using Polly;
using Polly.Retry;
using RabbitMQ.Client.Exceptions;
using SmtpStatusCode = MailKit.Net.Smtp.SmtpStatusCode;

namespace DevConnect.Infrastructure.Resilience;

public static class RetryPolicies
{
    public static AsyncRetryPolicy GetEmailRetryPolicy()
    {
        return Policy
            .Handle<SmtpCommandException>(IsTransient)
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: _ => TimeSpan.FromMinutes(5),
                onRetry: (exception, timeSpan, attempt, context) =>
                {
                    Console.WriteLine($"Retry {attempt}/5 after {timeSpan.TotalMinutes} min due to SMTP error: " +
                                      $"{(int)((SmtpCommandException)exception).StatusCode}");
                }
            );
    }

    public static AsyncRetryPolicy GetRabbitMqRetryPolicy()
    {
        return Policy
            .Handle<Exception>(IsTransientRabbitMqError)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, attempt, context) =>
                {
                    Console.WriteLine(
                        $"RabbitMQ publish retry {attempt}/3 after {timeSpan.TotalSeconds}s due to: {exception.Message}"
                    );
                }
            );
    }

    private static bool IsTransientRabbitMqError(Exception ex)
    {
        return ex is AlreadyClosedException or OperationInterruptedException;
    }

    private static bool IsTransient(SmtpCommandException ex)
    {
        Console.WriteLine($"SMTP error: {(int)ex.StatusCode} - {ex.StatusCode}");

        var transientStatusCode = new[]
        {
            SmtpStatusCode.MailboxBusy,
            SmtpStatusCode.ErrorInProcessing,
            SmtpStatusCode.InsufficientStorage,
            SmtpStatusCode.ServiceNotAvailable
        };
        
        return transientStatusCode.Contains(ex.StatusCode);
    }
}