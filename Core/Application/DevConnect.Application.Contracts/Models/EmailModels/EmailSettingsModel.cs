namespace DevConnect.Application.Contracts.Models.EmailModels;

public class EmailSettingsModel
{
    public string? SmtpHost { get; set; }
    public int SmtpPort { get; set; }
    public string? SenderName { get; set; }
    public string? SenderEmail { get; set; }
    public string? Username { get; set; }
}