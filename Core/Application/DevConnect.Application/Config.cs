using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Contracts.Models.EmailModels;
using DevConnect.Application.Mapping;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Auth.Helpers;
using DevConnect.Application.Services.Infra.Handlers;
using DevConnect.Infrastructure.Mailing;
using DevConnect.Infrastructure.Publisher;
using DevConnect.Infrastructure.PasswordHash;
using DevConnect.Infrastructure.Resilience;
using DevConnect.Infrastructure.Subscriber;
using DevConnect.Infrastructure.Subscriber.Interfaces;
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
        services.AddScoped<IUserAuthCommandRepository, UserAuthCommandRepository>();
        services.AddScoped<IUserAuthQueryRepository, UserAuthQueryRepository>();
        services.AddScoped<IVerificationResendCommandRepository, VerificationResendCommandRepository>();
        services.AddScoped<IVerificationResendQueryRepository, VerificationResendQueryRepository>();
    }
    
    public static void AddMappers(this IServiceCollection services)
    {
        services.AddScoped<IUserProfileMapper, UserProfileMapper>();
        services.AddScoped<IMessageMapper, MessageMapper>();
        services.AddScoped<INotificationMapper, NotificationMapper>();
        services.AddScoped<IAuthResultMapper, AuthResultMapper>();
        services.AddScoped<IRegisterUserMapper, RegisterUserMapper>();
        services.AddScoped<IVerificationResendMapper, VerificationResendMapper>();
    }

    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration builder)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.Configure<EmailSettingsModel>(builder.GetSection("EmailSettings"));
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
        services.AddTransient<DeadLetterDispatcher>();
        services.AddSingleton<IMessageHandler, EmailVerificationMessageHandler>();
        services.AddHostedService<RabbitMqSubscriber>();
        services.AddScoped<IMessagePublisher, RabbitMqPublisher>();
        services.AddScoped<JwtGenerator>();
    }

    public static void AddDbContexts(this IServiceCollection services, IConfiguration builder)
    {
        var psqlClient = builder.GetConnectionString("PostgresDb");

        services.AddDbContext<DevConnectDbContext>(options =>
            options.UseNpgsql(psqlClient, b => b.MigrationsAssembly("DevConnect.Persistence.DataModels")));

        var mongoClient = new MongoClient(builder.GetConnectionString("MongoDb"));

        services.AddDbContext<DevConnectMongoContext>(options =>
            options.UseMongoDB(mongoClient, "devconnect_db"));
    }
}