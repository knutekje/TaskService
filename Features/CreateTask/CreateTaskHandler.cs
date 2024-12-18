using MongoDB.Driver;
using TaskService.Infrastructure;
using TaskService.Infratstructure.MongoDb;
using TaskService.Models;


namespace TaskService.Features.CreateTask;



public class CreateTaskHandler
{
    private readonly MongoDbContext _dbContext;

    public CreateTaskHandler(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TaskEntity> Handle(CreateTaskRequest request)
    {
        var task = new TaskEntity
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Tasks.InsertOneAsync(task);
        return task;
    }
}
