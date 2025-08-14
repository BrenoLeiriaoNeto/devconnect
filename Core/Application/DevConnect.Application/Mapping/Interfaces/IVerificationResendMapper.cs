using DevConnect.Application.Contracts.Models.EmailModels;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping.Interfaces;

public interface IVerificationResendMapper
{
    VerificationResendMetadata ToDomain(VerificationResendInputModel input);
    VerificationResendViewModel ToViewModel(VerificationResendMetadata domain);
}