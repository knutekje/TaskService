using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskService.Features.CreateTask;
using Serilog;
using TaskService.Infrastructure.MongoDb;
using TaskService.Features.GetTasks;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext() 
    .WriteTo.Console() 
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) 
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*builder.Services.AddSingleton(sp =>
{
    var connectionString = "mongodb://debstar:27017"; // Adjust as needed
    var databaseName = "taskservice";
    return new MongoDbContext(connectionString, databaseName);
});*/


// Get MongoDB connection string from environment variables
var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");

if (string.IsNullOrEmpty(mongoConnectionString))
{
    throw new InvalidOperationException("MongoDB connection string is not set in the environment variables.");
}

// Configure MongoDB
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongoClient = new MongoClient(mongoConnectionString);
    return mongoClient.GetDatabase("taskdb"); // Replace with your database name
});


builder.Services.AddScoped<CreateTaskHandler>();
builder.Services.AddScoped<GetTaskHandler>();
//
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();


