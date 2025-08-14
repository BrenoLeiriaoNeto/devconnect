using DevConnect.Application.Contracts.Models.InputModels;
using MediatR;

namespace DevConnect.Application.Services.Auth.Commands;

public class RegisterUserCommand(RegisterUserInputModel input) : IRequest<Unit>
{
    public RegisterUserInputModel Input { get; } = input;
}