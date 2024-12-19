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

/*var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    mongoConnectionString = "mongodb://localhost:27017";
}
if (string.IsNullOrEmpty(mongoConnectionString))
{
    throw new InvalidOperationException("MongoDB connection string is not set in the environment variables.");
}*/
// Get MongoDB connection string from environment variables
/*var mongoConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    mongoConnectionString = "mongodb://localhost:27017";
}
if (string.IsNullOrEmpty(mongoConnectionString))
{
    throw new InvalidOperationException("MongoDB connection string is not set in the environment variables.");
}

// Configure MongoDB
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongoClient = new MongoClient(mongoConnectionString);
    return mongoClient.GetDatabase("taskdb"); 
});*/

builder.Services.AddSingleton<IMongoClient>(sp =>
{


    var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING"); 
    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
    {
        connectionString = "mongodb://debstar:27017";
        connectionString =
            "mongodb://root:rootpassword@mongodb.taskmanager.svc.cluster.local:27017/taskdb?authSource=admin";
    }
    //?? throw new InvalidOperationException("MONGO_CONNECTION_STRING environment variable is not set.");
   
    return new MongoClient(connectionString);
});

builder.Services.AddScoped(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var databaseName = "taskdb"; // Replace with your database name if needed
    return new MongoDbContext(mongoClient, databaseName);
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


