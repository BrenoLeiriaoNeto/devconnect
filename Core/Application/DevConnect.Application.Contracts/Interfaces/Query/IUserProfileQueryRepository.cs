using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Query;

public interface IUserProfileQueryRepository
{
    Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}