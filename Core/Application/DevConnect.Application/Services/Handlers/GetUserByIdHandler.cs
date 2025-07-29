using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Queries;
using DevConnect.Exceptions;
using MediatR;

namespace DevConnect.Application.Services.Handlers;

public class GetUserByIdHandler(
    IUserProfileQueryRepository userProfileQueryRepository,
    IUserProfileMapper userProfileMapper)
    : IRequestHandler<GetUserByIdQuery, UserProfileViewModel>
{
    public async Task<UserProfileViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userProfileQueryRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            throw new EntityNotFoundException($"User with ID '{request.UserId}' not found.");
        }
        
        var userViewModel = userProfileMapper.ToViewModel(user);

        return userViewModel;
    }
}