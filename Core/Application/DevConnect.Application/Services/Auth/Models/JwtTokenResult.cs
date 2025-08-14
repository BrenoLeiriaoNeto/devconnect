namespace DevConnect.Application.Services.Auth.Models;

public class JwtTokenResult
{
    public string Token { get; set; }
    public DateTime Expiry { get; set; }
}