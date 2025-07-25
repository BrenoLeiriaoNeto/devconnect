using DevConnect.Application.Contracts.Models.UpdateModels;
using MediatR;

namespace DevConnect.Application.Services.Commands;

public class UpdateUserProfileCommand(UserProfileUpdateModel update) : IRequest
{
    public UserProfileUpdateModel Update { get; }
}