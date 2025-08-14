using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;

namespace DevConnect.Persistence.Command;

public class VerificationResendCommandRepository(DevConnectDbContext psqlContext) 
    : IVerificationResendCommandRepository
{
    public async Task SaveVerificationResendAsync(VerificationResendMetadata metadata,
        CancellationToken cancellationToken)
    {
        await psqlContext.VerificationResendLogs.AddAsync(metadata, cancellationToken);
        await psqlContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateVerificationResendAsync(VerificationResendMetadata metadata, CancellationToken cancellationToken)
    {
        metadata.UpdateResendAttempt();
        psqlContext.VerificationResendLogs.Update(metadata);
        await psqlContext.SaveChangesAsync(cancellationToken);   
    }
}