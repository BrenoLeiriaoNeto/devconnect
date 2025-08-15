using DevConnect.Application.Contracts.Interfaces.Command;
using DevConnect.Application.Contracts.Interfaces.Common;
using DevConnect.Application.Contracts.Interfaces.Query;
using DevConnect.Application.Contracts.Models.EmailModels;
using DevConnect.Application.Mapping;
using DevConnect.Application.Mapping.Interfaces;
using DevConnect.Application.Services.Auth.Helpers;
using DevConnect.Application.Services.Infra.Handlers;
using DevConnect.Infrastructure.Mailing;
using DevConnect.Infrastructure.Models;
using DevConnect.Infrastructure.Publisher;
using DevConnect.Infrastructure.PasswordHash;
using DevConnect.Infrastructure.Resilience;
using DevConnect.Infrastructure.Subscriber;
using DevConnect.Infrastructure.Subscriber.Interfaces;
using DevConnect.Persistence.Command;
using DevConnect.Persistence.DataModels;
using DevConnect.Persistence.Query;
using Microsoft.EntityFrameworkCore;
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

    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.Configure<EmailSettingsModel>(options =>
        {
            options.SmtpHost = Environment.GetEnvironmentVariable("SMTP_HOST");
            options.SmtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT"));
            options.Username = Environment.GetEnvironmentVariable("SMTP_USERNAME");
            options.SenderEmail = Environment.GetEnvironmentVariable("SMTP_SENDER_EMAIL");
            options.SenderName = Environment.GetEnvironmentVariable("SMTP_SENDER_NAME");
        });
        services.Configure<RabbitMqSettingsModel>(options =>
        {
            options.HostName = Environment.GetEnvironmentVariable("RABBIT_MQ_HOST");
            options.Port = int.Parse(Environment.GetEnvironmentVariable("RABBIT_MQ_PORT"));
            options.UserName = Environment.GetEnvironmentVariable("RABBIT_MQ_USERNAME");
            options.Password = Environment.GetEnvironmentVariable("RABBIT_MQ_PASSWORD");
            options.QueueName = Environment.GetEnvironmentVariable("RABBIT_MQ_QUEUE");
            options.ExchangeName = Environment.GetEnvironmentVariable("RABBIT_MQ_EXCHANGE");
            options.ExchangeType = Environment.GetEnvironmentVariable("RABBIT_MQ_EXCHANGE_TYPE");
            options.RoutingKey = Environment.GetEnvironmentVariable("RABBIT_MQ_ROUTING_KEY");
        });
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
        services.AddHostedService<RabbitMqInitializer>();
        services.AddTransient<DeadLetterDispatcher>();
        services.AddSingleton<IMessageHandler, EmailVerificationMessageHandler>();
        services.AddHostedService<RabbitMqSubscriber>();
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddScoped<JwtGenerator>();
    }

    public static void AddDbContexts(this IServiceCollection services)
    {
        var psqlUsername = Environment.GetEnvironmentVariable("PSQL_USERNAME");
        var psqlPassword = Environment.GetEnvironmentVariable("PSQL_PASSWORD");
        var psqlDatabaseName = Environment.GetEnvironmentVariable("PSQL_DATABASE");
        var psqlPort = Environment.GetEnvironmentVariable("PSQL_PORT");
        var psqlHost = Environment.GetEnvironmentVariable("PSQL_HOST");

        var psqlConnection = 
            $"Host={psqlHost};" +
            $"Port={psqlPort};" +
            $"Database={psqlDatabaseName};" +
            $"Username={psqlUsername};" +
            $"Password={psqlPassword}";
        
        services.AddDbContext<DevConnectDbContext>(options =>
            options.UseNpgsql(psqlConnection, b => 
                b.MigrationsAssembly("DevConnect.Persistence.DataModels")));
        
        var mongoConnection = Environment.GetEnvironmentVariable("MONGO_URL");
        
        var mongoClient = new MongoClient(mongoConnection);
        
        services.AddDbContext<DevConnectMongoContext>(options =>
            options.UseMongoDB(mongoClient, "devconnect_db"));
    }
}