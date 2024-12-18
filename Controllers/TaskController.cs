using Microsoft.AspNetCore.Mvc;
using TaskService.Features.CreateTask;

namespace TaskService.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly CreateTaskHandler _createTaskHandler;

    public TasksController(CreateTaskHandler createTaskHandler)
    {
        _createTaskHandler = createTaskHandler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var task = await _createTaskHandler.Handle(request);
        return CreatedAtAction(nameof(CreateTask), new { id = task.Id }, task);
    }
}