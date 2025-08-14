using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Auth.Commands;
using DevConnect.Application.Services.Auth.Events;
using MediatR;

namespace DevConnect.Application.Services.Auth.Handlers;

public class RegisterUserCommandHandler(
    IUserAuthCommandRepository commandRepository,
    IUserProfileCommandRepository userProfileCommandRepository,
    IMediator mediator,
    IRegisterUserMapper mapper,
    IUserProfileMapper userProfileMapper,
    IPasswordHasher passwordHasher) : IRequestHandler<RegisterUserCommand, Unit>
{
    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = mapper.ToDomain(request.Input);
        newUser.CreatedAt = DateTime.UtcNow;
        newUser.CreatedBy = "System";
        newUser.PasswordHash = passwordHasher.HashPassword(newUser.PasswordHash);

        var profile = userProfileMapper.ToDomain(newUser.Username);
        profile.CreatedAt = DateTime.UtcNow;
        profile.CreatedBy = "System";
        await userProfileCommandRepository.AddUserAsync(profile, cancellationToken);
        
        newUser.ProfileId = profile.Id;
        
        await commandRepository.AddUserAuthAsync(newUser, cancellationToken);

        await mediator.Publish(new UserRegisteredEvent(profile.Id, request.Input.Email), cancellationToken);

        return Unit.Value;
    }
}