using DevConnect.Application.Contracts.Models.InputModels;
using MediatR;

namespace DevConnect.Application.Services.Commands;

public class CreateUserProfileCommand(UserProfileInputModel input) : IRequest
{
    public UserProfileInputModel Input { get; } = input;
}