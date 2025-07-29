namespace DevConnect.Application.Contracts.Models.UpdateModels;

public class UserProfileUpdateModel
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public string Location { get; set; }
}