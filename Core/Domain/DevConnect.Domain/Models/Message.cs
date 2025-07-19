namespace DevConnect.Domain.Models;

public class Message : BusinessObject
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Content { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; } = false;

    public Message(Guid senderId, Guid receiverId, string content)
    {
        SenderId = senderId;
        ReceiverId = receiverId;
        Content = content;
    }
    
    public void MarkAsRead()
    {
        IsRead = true;
        MarkUpdated(SenderId.ToString());
    }
}