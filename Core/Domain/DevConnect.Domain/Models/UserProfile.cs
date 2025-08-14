namespace DevConnect.Domain.Models;

public class UserProfile : BusinessObject
{
    public string DisplayName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? Location { get; set; }

    public UserProfile()
    {
        
    }

    public UserProfile(string displayName)
    {
        this.DisplayName = displayName;
    }
}