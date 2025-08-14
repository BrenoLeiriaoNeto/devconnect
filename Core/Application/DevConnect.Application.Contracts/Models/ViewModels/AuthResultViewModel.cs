namespace DevConnect.Application.Contracts.Models.ViewModels;

public class AuthResultViewModel
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiresAt { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }

    public Guid UserId { get; set; }
    public string Username { get; set; }
    public IEnumerable<string> Roles { get; set; } = [];

    public string? ErrorMessage { get; set; }
}