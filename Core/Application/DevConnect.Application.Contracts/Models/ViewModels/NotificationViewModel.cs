namespace DevConnect.Application.Contracts.Models.ViewModels;

public class NotificationViewModel
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Message { get; set; }
    public bool IsRead { get; set; } = false;
}