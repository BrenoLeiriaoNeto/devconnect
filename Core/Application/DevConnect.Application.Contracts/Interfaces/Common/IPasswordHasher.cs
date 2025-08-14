namespace DevConnect.Application.Contracts.Interfaces.Common;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}