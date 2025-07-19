namespace DevConnect.Domain.Models;

public class Notification : BusinessObject
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string? Message { get; set; }
    public bool IsRead { get; set; } = false;

    public Notification(Guid userId, string title, string? message = null)
    {
        UserId = userId;
        Title = title;
        Message = message;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        MarkUpdated(UserId.ToString());       
    }
}