using DevConnect.Application.Contracts.Models.InputModels;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping.Interfaces;

public interface IUserProfileMapper
{
    UserProfile ToDomain(UserProfileInputModel input);
    UserProfileViewModel ToViewModel(UserProfile domain);
}