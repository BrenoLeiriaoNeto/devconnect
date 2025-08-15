using System.Text;
using DevConnect.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DevConnect.WebApi.Middlewares;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context => throw (context.Exception switch
                {
                    SecurityTokenExpiredException => new TokenExpirationException("Token has expired."),
                    SecurityTokenInvalidSignatureException => new TokenValidationException(
                        "Invalid token signature."),
                    _ => new TokenValidationException("Token validation failed.")
                })
            };
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET"))),
                ClockSkew = TimeSpan.Zero
            };
        });
        return services;
    }
}