using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Commands;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class CreateUserProfileHandler : IRequestHandler<CreateUserProfileCommand>
{
    private readonly IUserProfileCommandRepository _userProfileCommandRepository;
    private readonly IUserProfileMapper _userProfileMapper;

    public CreateUserProfileHandler(IUserProfileCommandRepository userProfileCommandRepository,
        IUserProfileMapper userProfileMapper)
    {
        _userProfileCommandRepository = userProfileCommandRepository;
        _userProfileMapper = userProfileMapper;
    }
    
    public async Task Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = _userProfileMapper.ToDomain(request.Input);
        await _userProfileCommandRepository.AddUserAsync(userProfile);
    }
}