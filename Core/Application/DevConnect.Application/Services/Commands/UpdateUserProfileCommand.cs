using DevConnect.Application.Contracts.Models.UpdateModels;
using MediatR;

namespace DevConnect.Application.Services.Commands;

public class UpdateUserProfileCommand(UserProfileUpdateModel update, Guid id) : IRequest
{
    public UserProfileUpdateModel Update { get; init; }
    public Guid Id { get; init; }
}