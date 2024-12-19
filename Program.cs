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

var allowedOrigin = builder.Configuration["CORS_ALLOWED_ORIGIN"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

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
    Console.WriteLine(connectionString);
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
app.UseCors("AllowAll");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   
}



app.UseHttpsRedirection();

app.MapControllers();

app.Run();


