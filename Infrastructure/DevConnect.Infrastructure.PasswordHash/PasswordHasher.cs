using System.Security.Cryptography;
using DevConnect.Application.Contracts.Interfaces.Common;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DevConnect.Infrastructure.PasswordHash;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        byte[] salt = new byte[128 / 8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        
        var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        
        var saltString = Convert.ToBase64String(salt);

        return $"{saltString}.{hashed}";
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split(".");
        if (parts.Length != 2) return false;
        
        var salt = Convert.FromBase64String(parts[0]);
        var storedHash = parts[1];
        
        var hashToCompare = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        
        return storedHash == hashToCompare;
    }
}