using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Persistence.Query;

public class UserAuthQueryRepository(DevConnectDbContext psqlContext) : IUserAuthQueryRepository
{
    public async Task<UserAuth?> GetByEmailAsync(string email, CancellationToken cancellationToken) =>
        await psqlContext.UserAuths.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

    public async Task<UserAuth?> GetByUsernameAsync(string username, CancellationToken cancellationToken) => 
        await psqlContext.UserAuths.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

    public async Task<UserAuth?> GetByIdAsync(Guid id, CancellationToken cancellationToken) => 
        await psqlContext.UserAuths.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    public async Task<UserAuth?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken) =>
        await psqlContext.UserAuths.AsNoTracking()
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken 
                                      && u.RefreshTokenExpiry > DateTime.UtcNow, cancellationToken);

    public async Task<UserAuth?> GetByVerificationTokenAsync(string verificationToken,
        CancellationToken cancellationToken) => 
        await psqlContext.UserAuths.AsNoTracking()
            .FirstOrDefaultAsync(u => u.VerificationToken == verificationToken, cancellationToken);
}