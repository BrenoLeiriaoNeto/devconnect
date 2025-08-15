using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DevConnect.Application.Services.Auth.Models;
using DevConnect.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace DevConnect.Application.Services.Auth.Helpers;

public class JwtGenerator()
{
    public JwtTokenResult GenerateJwt(UserAuth userAuth)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userAuth.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, userAuth.Email),
            new Claim("username", userAuth.Username),
            new Claim("profileid", userAuth.ProfileId.ToString())
        };
        
        claims.AddRange(userAuth.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

        var expiry = DateTime.UtcNow.AddMinutes(30);
        
        var token = new JwtSecurityToken(
            issuer: Environment.GetEnvironmentVariable("JWT_ISSUER"),
            audience: Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
            claims: claims,
            expires: expiry,
            signingCredentials: credentials);
        
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtTokenResult
        {
            Token = tokenString,
            Expiry = expiry
        };
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}