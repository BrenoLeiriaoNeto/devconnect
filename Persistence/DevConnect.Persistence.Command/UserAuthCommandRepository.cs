using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Persistence.Command;

public class UserAuthCommandRepository(DevConnectDbContext psqlContext) : IUserAuthCommandRepository
{
    public async Task AddUserAuthAsync(UserAuth userAuth, CancellationToken cancellationToken)
    {
        await psqlContext.UserAuths.AddAsync(userAuth, cancellationToken);
        await psqlContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserAuthAsync(UserAuth userAuth, CancellationToken cancellationToken)
    {
        psqlContext.UserAuths.Update(userAuth);
        await psqlContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken)
    {
        return await psqlContext.UserAuths.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken)
    {
        return await psqlContext.UserAuths.AnyAsync(u => u.Username == username, cancellationToken);
    }

    public async Task<bool> VerifyEmailAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await psqlContext.UserAuths.FirstAsync(u => u.ProfileId == userId, cancellationToken);

        if (user is null || user.IsVerified)
        {
            return false;
        }

        user.IsVerified = true;
        user.VerifiedAt = DateTime.UtcNow;
        
        await psqlContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}