using Contracts;
using ServiceBus.Abstractions;

namespace TodoApi.EventHandling;

public class ProjectEventHandler: IIntegrationEventHandler<ProjectEvent>
{
    public Task Handle(ProjectEvent @event)
    {
        Console.WriteLine($"Handling project event inside todo api: {@event.ProjectId}");
        return Task.CompletedTask;
    }
}