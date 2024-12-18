namespace TaskService.Features.CreateTask;

public record TaskCreatedEvent(Guid TaskId, string Title, string Status);