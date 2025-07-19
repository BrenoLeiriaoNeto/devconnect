namespace DevConnect.Domain.Models;

public class UserAuth : BusinessObject
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public bool IsVerified { get; set; }

    public Guid ProfileId { get; set; }
    public UserProfile Profile { get; set; }
}