namespace TaskService.Infrastructure.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event);
}