using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Auth.Commands;
using DevConnect.Application.Services.Auth.Helpers;
using DevConnect.Application.Services.Auth.Models;
using DevConnect.Exceptions;
using MediatR;
using UnauthorizedAccessException = DevConnect.Exceptions.UnauthorizedAccessException;

namespace DevConnect.Application.Services.Auth.Handlers;

public class LoginUserCommandHandler(
    IUserAuthQueryRepository queryRepository,
    IUserAuthCommandRepository commandRepository,
    IAuthResultMapper mapper,
    IPasswordHasher passwordHasher,
    JwtGenerator jwtGenerator)
    : IRequestHandler<LoginUserCommand, AuthResultViewModel>
{
    public async Task<AuthResultViewModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await queryRepository.GetByEmailAsync(request.Email, cancellationToken)
            ?? await queryRepository.GetByUsernameAsync(request.Email, cancellationToken);

        if (user == null || !passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException("Invalid credentials");
        }

        if (!user.IsVerified)
        {
            throw new UnauthorizedAccessException("Email not verified");
        }
        
        if (user.IsLocked && user.LockoutEnd > DateTime.UtcNow)
        {
            throw new ForbiddenOperationException("Account locked until " + user.LockoutEnd);
        }

        var jwt = jwtGenerator.GenerateJwt(user);
        var refreshToken = jwtGenerator.GenerateRefreshToken();
        var refreshExpiry = DateTime.UtcNow.AddDays(7);
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = refreshExpiry;
        user.LastLoginAt = DateTime.UtcNow;
        user.FailedLoginCount = 0;

        await commandRepository.UpdateUserAuthAsync(user, cancellationToken);
        
        var result = AuthResult.Success(
            token: jwt.Token,
            expiry: jwt.Expiry,
            refreshToken: refreshToken,
            refreshExpiry: refreshExpiry,
            userId: user.Id,
            username: user.Username,
            roles: user.Roles.Select(r => r.Name)
            );
        
        return mapper.ToViewModel(result);
    }
}