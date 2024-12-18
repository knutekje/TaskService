using MongoDB.Driver;
using TaskService.Infrastructure.MongoDb;
using TaskService.Models;

namespace TaskService.Features.GetTasks;

public class GetTaskHandler
{
    private readonly MongoDbContext _dbContext;

    public GetTaskHandler(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TaskEntity> Handle(Guid taskId)
    {
        var task = await _dbContext.Tasks.Find(t => t.Id == taskId).FirstOrDefaultAsync();
        return task;
    }

    public async Task<IEnumerable<TaskEntity>> Handle()
    {
        var tasks = await _dbContext.Tasks.Find(t => true).ToListAsync();
        return tasks;
    }
}