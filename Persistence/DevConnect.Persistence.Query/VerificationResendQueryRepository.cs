using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Persistence.Query;

public class VerificationResendQueryRepository(DevConnectDbContext psqlContext) : IVerificationResendQueryRepository
{
    public async Task<VerificationResendMetadata?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken) => 
        await psqlContext.VerificationResendLogs.AsNoTracking()
            .FirstOrDefaultAsync(v => v.UserId == userId, cancellationToken);
}