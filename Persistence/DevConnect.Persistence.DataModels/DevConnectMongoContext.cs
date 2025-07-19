using DevConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace DevConnect.Persistence.DataModels;

public class DevConnectMongoContext : DbContext
{
    public DevConnectMongoContext(DbContextOptions<DevConnectMongoContext> options) : base(options)
    {
    }

    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Notification> Notifications => Set<Notification>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DevConnectMongoContext).Assembly);
    }
}