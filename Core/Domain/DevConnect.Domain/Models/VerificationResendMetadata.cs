namespace DevConnect.Domain.Models;

public class VerificationResendMetadata(Guid userId)
{
    public Guid UserId { get; set; } = userId;
    public DateTime LastSentAt { get; set; } = DateTime.UtcNow;
    public int AttemptCount { get; set; } = 1;

    public void UpdateResendAttempt()
    {
        LastSentAt = DateTime.UtcNow;
        AttemptCount++;
    }
    
    public bool IsInCooldown(TimeSpan cooldown)
    {
        return DateTime.UtcNow - LastSentAt < cooldown;
    }
}