using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Command;

public interface IUserAuthCommandRepository
{
    Task AddUserAuthAsync(UserAuth userAuth, CancellationToken cancellationToken);
    Task UpdateUserAuthAsync(UserAuth userAuth, CancellationToken cancellationToken);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken);
    Task<bool> VerifyEmailAsync(Guid userId, CancellationToken cancellationToken);
}