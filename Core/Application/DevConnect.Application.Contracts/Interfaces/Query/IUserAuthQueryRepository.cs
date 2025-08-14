using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Query;

public interface IUserAuthQueryRepository
{
    Task<UserAuth?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<UserAuth?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<UserAuth?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<UserAuth?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken);
    Task<UserAuth?> GetByVerificationTokenAsync(string verificationToken, CancellationToken cancellationToken);
}