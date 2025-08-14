namespace DevConnect.Application.Contracts.Models.InputModels;

public class RegisterUserInputModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}