using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Application.Contracts.Models.EmailModels;
using DevConnect.Infrastructure.Resilience;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorLight;

namespace DevConnect.Infrastructure.Mailing;

public class SmtpEmailSender(IOptions<EmailSettingsModel> options, 
    DeadLetterDispatcher deadLetterDispatcher) : IEmailSender
{
    private readonly EmailSettingsModel _emailSettings = options.Value;
    private readonly string _smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD")
                                            ?? throw new InvalidOperationException
                                                ("SMTP_PASSWORD is not set in environment variables.");

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(Path.Combine(AppContext.BaseDirectory, "Assets"))
            .UseMemoryCachingProvider()
            .Build();

        var model = new VerifyEmailModel
        {
            Name = to.Split('@')[0],
            VerificationUrl = body
        };
        
        var htmlBody = await engine.CompileRenderAsync("VerifyEmailTemplate.cshtml", model);
        
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        
        var builder = new BodyBuilder();
        
        var logoPath = Path.Combine(AppContext.BaseDirectory, "Assets", "DevConnectLogo.png");
        var logo = await builder.LinkedResources.AddAsync(logoPath);
        logo.ContentId = "DevConnectLogo";
        
        builder.HtmlBody = htmlBody;
        message.Body = builder.ToMessageBody();

        var retryPolicy = RetryPolicies.GetEmailRetryPolicy();

        try
        {
            await retryPolicy.ExecuteAsync(async () =>
            {
                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort,
                    SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Username, _smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            });
        }
        catch (SmtpCommandException e)
        {
            Console.WriteLine($"Permanent SMTP failure: {e.StatusCode} - {e.Message}");
            await deadLetterDispatcher.PublishToDeadLetterQueueAsync(model);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Permanent SMTP failure: {e.Message}");
            await deadLetterDispatcher.PublishToDeadLetterQueueAsync(model);
        }
    }
}