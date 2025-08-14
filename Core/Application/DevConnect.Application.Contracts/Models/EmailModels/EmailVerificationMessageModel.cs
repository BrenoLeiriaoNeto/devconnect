namespace DevConnect.Application.Contracts.Models.EmailModels;

public class EmailVerificationMessageModel
{
    public string Email { get; set; }
    public string VerificationToken { get; set; }
}