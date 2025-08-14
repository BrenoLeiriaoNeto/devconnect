namespace DevConnect.Application.Services.Auth.Models;

public class AuthResult
{
    public bool IsSuccess { get; set; }
    public string Token { get; set; }
    public DateTime Expiry { get; set; }

    public string RefreshToken { get; set; }
    public DateTime RefreshExpiry { get; set; }

    public Guid UserId { get; set; }
    public string Username { get; set; }
    public IEnumerable<string> Roles { get; set; } = [];

    public string? Error { get; set; }

    public static AuthResult Success(
        string token,
        DateTime expiry,
        string refreshToken,
        DateTime refreshExpiry,
        Guid userId,
        string username,
        IEnumerable<string> roles)
    {
        return new AuthResult
        {
            IsSuccess = true,
            Token = token,
            Expiry = expiry,
            RefreshToken = refreshToken,
            RefreshExpiry = refreshExpiry,
            UserId = userId,
            Username = username,
            Roles = roles
        };
    }

    public static AuthResult Failure(string error)
    {
        return new AuthResult
        {
            IsSuccess = false,
            Error = error
        };
    }
}