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
    
    public async Task UpdateUserAsync(UserProfile userProfile)
    {
        _psqlContext.UserProfiles.Update(userProfile);
        await _psqlContext.SaveChangesAsync();
    }

    public async Task AddUserAsync(UserProfile userProfile)
    {
        await _psqlContext.UserProfiles.AddAsync(userProfile);
        await _psqlContext.SaveChangesAsync();
    }
}