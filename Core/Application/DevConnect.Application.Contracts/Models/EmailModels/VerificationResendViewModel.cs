namespace DevConnect.Application.Contracts.Models.EmailModels;

public class VerificationResendViewModel
{
    public Guid UserId { get; set; }
    public DateTime LastSentAt { get; set; }
    public int AttemptCount { get; set; }
}