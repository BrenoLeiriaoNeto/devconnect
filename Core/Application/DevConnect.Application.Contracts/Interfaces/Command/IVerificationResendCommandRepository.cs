using DevConnect.Domain.Models;

namespace DevConnect.Application.Contracts.Interfaces.Command;

public interface IVerificationResendCommandRepository
{
    Task SaveVerificationResendAsync(VerificationResendMetadata metadata, CancellationToken cancellationToken);
    Task UpdateVerificationResendAsync(VerificationResendMetadata metadata, CancellationToken cancellationToken);
}