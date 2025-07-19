using DevConnect.Persistence.DataModels;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

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

var psqlClient = builder.Configuration.GetConnectionString("PostgresDb");

builder.Services.AddDbContext<DevConnectDbContext>(options =>
    options.UseNpgsql(psqlClient));

var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoDb"));

builder.Services.AddDbContext<DevConnectMongoContext>(options =>
    options.UseMongoDB(mongoClient, "devconnect_db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DevConnectDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowViteFrontend");

app.Run();
