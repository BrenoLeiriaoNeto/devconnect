namespace DevConnect.Application.Contracts.Models.UpdateModels;

public class UserProfileUpdateModel
{
    public string DisplayName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string Location { get; set; }
}