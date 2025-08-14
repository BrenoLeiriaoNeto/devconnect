using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Query;

public interface IVerificationResendQueryRepository
{
    Task<VerificationResendMetadata?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}