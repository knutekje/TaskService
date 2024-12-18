using Microsoft.AspNetCore.Mvc;
using TaskService.Features.CreateTask;
using TaskService.Features.GetTasks;

namespace TaskService.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly CreateTaskHandler _createTaskHandler;
    private readonly GetTaskHandler _getTaskHandler;

    public TasksController(CreateTaskHandler createTaskHandler, GetTaskHandler getTaskHandler)
    {
        _createTaskHandler = createTaskHandler;
        _getTaskHandler = getTaskHandler;
    }
   

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequest request)
    {
        var task = await _createTaskHandler.Handle(request);
        return CreatedAtAction(nameof(CreateTask), new { id = task.Id }, task);
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _getTaskHandler.Handle();
        return Ok(tasks);
    }


}