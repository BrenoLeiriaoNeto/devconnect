using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Command;

public interface IUserProfileCommandRepository
{
    Task UpdateUserAsync(UserProfile userProfile, CancellationToken cancellationToken);
    Task AddUserAsync(UserProfile userProfile, CancellationToken cancellationToken);
}