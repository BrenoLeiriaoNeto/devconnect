namespace DevConnect.Application.Contracts.Models.UpdateModels;

public class MessageUpdateModel
{
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}