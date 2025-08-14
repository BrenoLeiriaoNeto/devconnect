using DevConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Persistence.DataModels;

public class DevConnectDbContext : DbContext
{
    public DevConnectDbContext(DbContextOptions<DevConnectDbContext> options) : base(options)
    {
    }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<UserAuth> UserAuths => Set<UserAuth>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<VerificationResendMetadata> VerificationResendLogs => Set<VerificationResendMetadata>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Ignore<Message>();
        modelBuilder.Ignore<Notification>();
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DevConnectDbContext).Assembly);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()?.ToLowerInvariant());
        }
    }
}