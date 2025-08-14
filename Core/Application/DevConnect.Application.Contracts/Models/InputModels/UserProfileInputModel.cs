namespace DevConnect.Application.Contracts.Models.InputModels;

public class UserProfileInputModel
{
    public string DisplayName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string? Location { get; set; }
}