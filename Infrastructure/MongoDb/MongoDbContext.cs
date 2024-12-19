using MongoDB.Driver;
using TaskService.Models;

namespace TaskService.Infrastructure.MongoDb;


using MongoDB.Driver;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoClient mongoClient, string databaseName)
    {
        _database = mongoClient.GetDatabase(databaseName);
    }

    public IMongoCollection<TaskEntity> Tasks => _database.GetCollection<TaskEntity>("Tasks");
}
