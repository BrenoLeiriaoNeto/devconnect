using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Domain.Models;
using DevConnect.Persistence.DataModels;

namespace DevConnect.Persistence.Command;

public class UserProfileCommandRepository : IUserProfileCommandRepository
{
    private readonly DevConnectDbContext _psqlContext;

    public UserProfileCommandRepository(DevConnectDbContext psqlContext)
    {
        _psqlContext = psqlContext;
    }
    
    public async Task UpdateUserAsync(UserProfile userProfile, CancellationToken cancellationToken)
    {
        _psqlContext.UserProfiles.Update(userProfile);
        await _psqlContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddUserAsync(UserProfile userProfile, CancellationToken cancellationToken)
    {
        await _psqlContext.UserProfiles.AddAsync(userProfile, cancellationToken);
        await _psqlContext.SaveChangesAsync(cancellationToken);
    }
}