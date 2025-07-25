using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Queries;
using DevConnect.Exceptions;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserProfileViewModel>
{
    private readonly IUserProfileQueryRepository _userProfileQueryRepository;
    private readonly IUserProfileMapper _userProfileMapper;

    public GetUserByIdHandler(IUserProfileQueryRepository userProfileQueryRepository, 
        IUserProfileMapper userProfileMapper)
    {
        _userProfileQueryRepository = userProfileQueryRepository;
        _userProfileMapper = userProfileMapper;
    }
    
    public async Task<UserProfileViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userProfileQueryRepository.GetByIdAsync(request.UserId);
        if (user is null)
        {
            throw new EntityNotFoundException($"User with ID '{request.UserId}' not found.");
        }
        
        var userViewModel = _userProfileMapper.ToViewModel(user);

        return userViewModel;
    }
}