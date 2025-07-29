using DevConnect.Application;
using DevConnect.Application.Behaviors;
using DevConnect.Application.Contracts.Validations;
using DevConnect.Application.Services.Handlers;
using DevConnect.Persistence.DataModels;
using DevConnect.WebApi.Middlewares;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowViteFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddDbContexts(builder.Configuration);

builder.Services.AddRepositories();
builder.Services.AddValidatorsFromAssemblyContaining<UserProfileInputModelValidator>();
builder.Services.AddMappers();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateUserProfileHandler>();
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowViteFrontend");

app.MapControllers();

app.Run();
