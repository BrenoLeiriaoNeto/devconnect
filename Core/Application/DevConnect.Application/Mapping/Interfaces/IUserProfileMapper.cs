using DevConnect.Application.Contracts.Models.InputModels;
using DevConnect.Application.Contracts.Models.UpdateModels;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping.Interfaces;

public interface IUserProfileMapper
{
    UserProfile ToDomain(UserProfileInputModel input);
    UserProfile ToDomain(UserProfileUpdateModel update);
    UserProfileViewModel ToViewModel(UserProfile domain);
}