namespace DevConnect.Application.Contracts.Models.EmailModels;

public class EmailSettingsModel
{
    public string SmtpHost { get; set; } = "in-v3.mailjet.com";
    public int SmtpPort { get; set; } = 587;
    public string SenderName { get; set; } = "DevConnect";
    public string SenderEmail { get; set; } = "devconnectapp@outlook.com";
    public string Username { get; set; } = "dc9bf354a319f1914288a58ae2036242";
}