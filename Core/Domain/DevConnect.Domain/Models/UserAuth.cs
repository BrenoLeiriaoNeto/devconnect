namespace DevConnect.Domain.Models;

public class UserAuth : BusinessObject
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    
    public bool IsVerified { get; set; }
    public string? VerificationToken { get; set; }
    public DateTime? VerificationExpiry { get; set; }
    public DateTime? VerifiedAt { get; set; }

    public bool IsTwoFactorEnabled { get; set; }
    public string? TwoFactorSecret { get; set; }

    public bool IsLocked { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public int FailedLoginCount { get; set; }
    public DateTime? LastLoginAt { get; set; }

    public string? RefreshToken { get; set; }
    public string? RefreshTokenId { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    public ICollection<Role> Roles { get; set; } = new List<Role>();

    public Guid ProfileId { get; set; }
    public UserProfile Profile { get; set; }

    public UserAuth()
    {
        
    }
    public UserAuth(string username, string email, string passwordHash)
    {
        this.Username = username;
        this.Email = email;
        this.PasswordHash = passwordHash;
    }
}