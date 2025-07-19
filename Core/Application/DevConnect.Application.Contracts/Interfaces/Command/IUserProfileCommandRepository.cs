using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Command;

public interface IUserProfileCommandRepository
{
    Task UpdateAsync(UserProfile userProfile);
}