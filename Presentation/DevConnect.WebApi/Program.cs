using DevConnect.Application;
using DevConnect.Application.Behaviors;
using DevConnect.Application.Contracts.Validations;
using DevConnect.Application.Services.Auth.Handlers;
using DevConnect.Application.Services.Handlers;
using DevConnect.Application.Services.Infra.Handlers;
using DevConnect.Persistence.DataModels;
using DevConnect.WebApi.Middlewares;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
    
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/devconnect.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContexts();

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddRepositories();
builder.Services.AddInfrastructureServices();
builder.Services.AddValidatorsFromAssemblyContaining<UserProfileInputModelValidator>();
builder.Services.AddMappers();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<LoginUserCommandHandler>();
    cfg.RegisterServicesFromAssemblyContaining<UpdateUserProfileHandler>();
    cfg.RegisterServicesFromAssemblyContaining<EmailVerificationCommandHandler>();
});
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DevConnectDbContext>();
    dbContext.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowViteFrontend");

app.MapControllers();

app.Run();
