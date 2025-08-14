using DevConnect.Application.Contracts.Models.InputModels;
using DevConnect.Application.Contracts.Models.UpdateModels;
using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping;

public class UserProfileMapper : IUserProfileMapper
{
    public UserProfile ToDomain(string displayName)
    {
        return new UserProfile
        {
            DisplayName = displayName
        };
    }
    
    public UserProfile ToDomain(UserProfileUpdateModel update)
    {
        return new UserProfile
        {
            DisplayName = update.DisplayName,
            ProfilePictureUrl = update.ProfilePictureUrl,
            Bio = update.Bio,
            Location = update.Location,
        };
    }

    public UserProfileViewModel ToViewModel(UserProfile domain)
    {
        return new UserProfileViewModel
        {
            DisplayName = domain.DisplayName,
            ProfilePictureUrl = domain.ProfilePictureUrl,
            Bio = domain.Bio,
            Location = domain.Location,
        };
    }
}