using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Application.Services.Auth.Models;

namespace DevConnect.Application.Mapping.Interfaces;

public interface IAuthResultMapper
{
    AuthResultViewModel ToViewModel(AuthResult authResult);
}