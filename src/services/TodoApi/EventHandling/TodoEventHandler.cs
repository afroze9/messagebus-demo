using Contracts;
using ServiceBus.Abstractions;

namespace TodoApi.EventHandling;

public class TodoEventHandler : IIntegrationEventHandler<TodoEvent>
{
    public Task Handle(TodoEvent @event)
    {
        Console.WriteLine($"Handling todo event: {@event.TodoId}");
        return Task.CompletedTask;
    }
}