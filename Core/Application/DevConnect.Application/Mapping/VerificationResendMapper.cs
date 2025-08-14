using DevConnect.Application.Contracts.Models.EmailModels;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Domain.Models;

namespace DevConnect.Application.Mapping;

public class VerificationResendMapper : IVerificationResendMapper
{
    public VerificationResendMetadata ToDomain(VerificationResendInputModel input)
    {
        return new VerificationResendMetadata(input.UserId);
    }

    public VerificationResendViewModel ToViewModel(VerificationResendMetadata domain)
    {
        return new VerificationResendViewModel();
    }
}