using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Application.Services.Auth.Events;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DevConnect.Application.Services.Auth.Handlers;

public class UserRegisteredEventHandler(
    IConfiguration configuration,
    IEmailSender emailSender,
    ILogger<UserRegisteredEventHandler> logger
    ) : INotificationHandler<UserRegisteredEvent>
{
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, notification.UserId.ToString())
            ]),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var verificationToken = tokenHandler.WriteToken(token);

        var baseUrl = configuration["App:BaseUrl"];
        var verificationLink = $"{baseUrl}/api/auth/verify-email?token={verificationToken}";

        await emailSender.SendEmailAsync(notification.Email,
            "Verify your DevConnect account", verificationLink);
        
        logger.LogInformation("Verification email sent to {Email}", notification.Email);
    }
}