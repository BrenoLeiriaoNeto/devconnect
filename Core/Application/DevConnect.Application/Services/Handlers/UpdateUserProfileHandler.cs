using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Commands;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class UpdateUserProfileHandler(
    IUserProfileCommandRepository userProfileCommandRepository,
    IUserProfileMapper userProfileMapper)
    : IRequestHandler<UpdateUserProfileCommand>
{
    public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        if (request.Id != request.Update.Id)
            throw new ArgumentException("User ID in path and body must match.");
        
        var userProfile = userProfileMapper.ToDomain(request.Update);
        await userProfileCommandRepository.UpdateUserAsync(userProfile, cancellationToken);
    }
}