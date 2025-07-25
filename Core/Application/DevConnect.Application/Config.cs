using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Mapping;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Persistence.Command;
using DevConnect.Persistence.DataModels;
using DevConnect.Persistence.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DevConnect.Application;

public static class Config
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserProfileCommandRepository, UserProfileCommandRepository>();
        services.AddScoped<IMessageCommandRepository, MessageCommandRepository>();
        services.AddScoped<INotificationCommandRepository, NotificationCommandRepository>();
        services.AddScoped<IUserProfileQueryRepository, UserProfileQueryRepository>();
        services.AddScoped<IMessageQueryRepository, MessageQueryRepository>();
        services.AddScoped<INotificationQueryRepository, NotificationQueryRepository>();
    }
    
    public static void AddMappers(this IServiceCollection services)
    {
        services.AddScoped<IUserProfileMapper, UserProfileMapper>();
        services.AddScoped<IMessageMapper, MessageMapper>();
        services.AddScoped<INotificationMapper, NotificationMapper>();
    }

    public static void AddDbContexts(this IServiceCollection services, IConfiguration builder)
    {
        var psqlClient = builder.GetConnectionString("PostgresDb");

        services.AddDbContext<DevConnectDbContext>(options =>
            options.UseNpgsql(psqlClient));

        var mongoClient = new MongoClient(builder.GetConnectionString("MongoDb"));

        services.AddDbContext<DevConnectMongoContext>(options =>
            options.UseMongoDB(mongoClient, "devconnect_db"));
    }
}