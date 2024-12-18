using MongoDB.Driver;
using TaskService.Infrastructure.MongoDb;
using TaskService.Models;

namespace TaskService.Features.DeleteTask;

public class DeleteTaskHandler
{
    private readonly MongoDbContext _dbContext;

    public DeleteTaskHandler(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(Guid taskId)
    {
        var result = await _dbContext.Tasks.DeleteOneAsync(t => t.Id == taskId);
        return result.DeletedCount > 0;
    }
}