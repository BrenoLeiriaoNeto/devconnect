using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Services.Infra.Commands;
using DevConnect.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DevConnect.Application.Services.Infra.Handlers;

public class EmailVerificationCommandHandler(
    ILogger<EmailVerificationCommandHandler> logger,
    IUserAuthCommandRepository commandRepository
    ) : IRequestHandler<EmailVerificationCommand, bool>
{
    public async Task<bool> Handle(EmailVerificationCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.VerificationToken))
        {
            throw new TokenValidationException("Email Verification token is required");
        }
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            var principal = tokenHandler.ValidateToken(request.VerificationToken, parameters, out _);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || string.IsNullOrWhiteSpace(userIdClaim.Value))
            {
                throw new UserNotFoundException("UserId not found in token");
            }
            var userId = userIdClaim.Value;

            var success = await commandRepository.VerifyEmailAsync(Guid.Parse(userId), cancellationToken);

            if (success)
            {
                logger.LogInformation("Email verified for user {UserId}", userId);
            }
            else
            {
                throw new EmailValidationException($"Failed to verify email for user {userId}");
            }

            return success;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to validate verification token");
            return false;
        }
    }
}