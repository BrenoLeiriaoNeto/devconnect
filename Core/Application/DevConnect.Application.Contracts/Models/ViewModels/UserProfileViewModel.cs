namespace DevConnect.Application.Contracts.Models.ViewModels;

public class UserProfileViewModel
{
    public string DisplayName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string Location { get; set; } = string.Empty;
}