using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Commands;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class CreateUserProfileHandler(
    IUserProfileCommandRepository userProfileCommandRepository,
    IUserProfileMapper userProfileMapper)
    : IRequestHandler<CreateUserProfileCommand>
{
    public async Task Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = userProfileMapper.ToDomain(request.Input);
        userProfile.CreatedAt = DateTime.UtcNow;
        userProfile.CreatedBy = "Test User";
        await userProfileCommandRepository.AddUserAsync(userProfile, cancellationToken);
    }
}