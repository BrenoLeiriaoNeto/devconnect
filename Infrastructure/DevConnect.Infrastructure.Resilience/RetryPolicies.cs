using MailKit.Net.Smtp;
using Polly;
using Polly.Retry;
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