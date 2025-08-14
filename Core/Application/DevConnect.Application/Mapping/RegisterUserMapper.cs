using DevConnect.Application.Contracts.Models.InputModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping;

public class RegisterUserMapper : IRegisterUserMapper
{
    public UserAuth ToDomain(RegisterUserInputModel input)
    {
        return new UserAuth(input.Username, input.Email, input.PasswordHash);
    }
}