using MongoDB.Driver;
using TaskService.Models;

namespace TaskService.Infrastructure.MongoDb;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    //public IMongoCollection<TaskEntity> Tasks => _database.GetCollection<TaskEntity>("tasks");
    public virtual IMongoCollection<TaskEntity> Tasks => _database.GetCollection<TaskEntity>("tasks");
}
