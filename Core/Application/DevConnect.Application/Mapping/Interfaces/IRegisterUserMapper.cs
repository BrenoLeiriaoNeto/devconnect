using DevConnect.Application.Contracts.Models.InputModels;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping.Interfaces;

public interface IRegisterUserMapper
{
    UserAuth ToDomain(RegisterUserInputModel input);
}