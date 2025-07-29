using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Persistence.Query;

public class UserProfileQueryRepository : IUserProfileQueryRepository
{
    private readonly DevConnectDbContext _psqlContext;

    public UserProfileQueryRepository(DevConnectDbContext psqlContext)
    {
        _psqlContext = psqlContext;
    }
    public async Task<UserProfile?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _psqlContext.UserProfiles
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }
}