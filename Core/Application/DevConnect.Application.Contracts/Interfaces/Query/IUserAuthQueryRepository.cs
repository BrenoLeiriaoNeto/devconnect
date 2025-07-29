using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Query;

public interface IUserAuthQueryRepository
{
    Task<UserAuth?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<UserAuth?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}