using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Commands;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand>
{
    private readonly IUserProfileCommandRepository _userProfileCommandRepository;
    private readonly IUserProfileMapper _userProfileMapper;

    public UpdateUserProfileHandler(IUserProfileCommandRepository userProfileCommandRepository)
    {
        _userProfileCommandRepository = userProfileCommandRepository;
    }
    
    public async Task Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = _userProfileMapper.ToDomain(request.Update);
        await _userProfileCommandRepository.UpdateUserAsync(userProfile);
    }
}