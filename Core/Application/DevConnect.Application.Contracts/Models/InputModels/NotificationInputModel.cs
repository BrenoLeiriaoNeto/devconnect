namespace DevConnect.Application.Contracts.Models.InputModels;

public class NotificationInputModel
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Message { get; set; }
    public bool IsRead { get; set; } = false;
}