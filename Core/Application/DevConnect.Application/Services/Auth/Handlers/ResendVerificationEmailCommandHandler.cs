using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Contracts.Models.EmailModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Auth.Commands;
using DevConnect.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DevConnect.Application.Services.Auth.Handlers;

public class ResendVerificationEmailCommandHandler(
    IUserAuthQueryRepository userAuthQueryRepository,
    IVerificationResendQueryRepository verificationResendQueryRepository,
    IVerificationResendCommandRepository verificationResendCommandRepository,
    IVerificationResendMapper mapper,
    IMessagePublisher messagePublisher,
    ILogger<ResendVerificationEmailCommandHandler> logger
    ) : IRequestHandler<ResendVerificationEmailCommand>
{
    public async Task Handle(ResendVerificationEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userAuthQueryRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException($"User with ID '{request.UserId}' not found.");
        }

        if (user.IsVerified)
        {
            throw new EmailValidationException("Email is already verified.");
        }
        
        var metadata = await verificationResendQueryRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        var cooldown = TimeSpan.FromMinutes(5);
        
        if (metadata is not null && metadata.IsInCooldown(cooldown))
            throw new EmailValidationException("Please wait before resending verification email.");

        if (metadata is null)
        {
            var input = new VerificationResendInputModel { UserId = request.UserId };
            var newMetadata = mapper.ToDomain(input);
            await verificationResendCommandRepository.SaveVerificationResendAsync(newMetadata, cancellationToken);
        }
        else
        {
            await verificationResendCommandRepository.UpdateVerificationResendAsync(metadata, cancellationToken);
        }

        var token = GenerateVerificationToken(user.Id);

        var message = new EmailVerificationMessageModel { VerificationToken = token };
        
        await messagePublisher.PublishMessageAsync(message);
        
        logger.LogInformation("Verification email resent to {Email}", user.Email);
    }
    private string GenerateVerificationToken(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
            ]),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}