using DevConnect.Application.Contracts.Models.ViewModels;
using MediatR;

namespace DevConnect.Application.Services.Auth.Commands;

public class LoginUserCommand(string email, string password) : IRequest<AuthResultViewModel>
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}