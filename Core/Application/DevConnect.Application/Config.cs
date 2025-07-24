using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Mapping;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Persistence.Command;
using DevConnect.Persistence.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevConnect.Application;

public static class Config
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configurations)
    {
            
    }

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
}