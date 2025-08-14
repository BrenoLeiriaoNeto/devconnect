using DevConnect.Application.Contracts.Models.ViewModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Auth.Models;

namespace DevConnect.Application.Mapping;

public class AuthResultMapper : IAuthResultMapper
{
    public AuthResultViewModel ToViewModel(AuthResult result)
    {
        if (!result.IsSuccess)
        {
            return new AuthResultViewModel
            {
                ErrorMessage = result.Error
            };
        }
        return new AuthResultViewModel
        {
            AccessToken = result.Token,
            AccessTokenExpiresAt = result.Expiry,
            RefreshToken = result.RefreshToken,
            RefreshTokenExpiresAt = result.RefreshExpiry,
            UserId = result.UserId,
            Username = result.Username,
            Roles = result.Roles
        };
    }
}