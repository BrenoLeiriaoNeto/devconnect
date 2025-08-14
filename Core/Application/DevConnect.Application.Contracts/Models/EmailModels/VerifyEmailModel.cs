namespace DevConnect.Application.Contracts.Models.EmailModels;

public class VerifyEmailModel
{
    public string Name { get; set; }
    public string VerificationUrl { get; set; }
}