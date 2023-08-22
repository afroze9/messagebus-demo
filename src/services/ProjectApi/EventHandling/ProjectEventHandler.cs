using Contracts;
using ServiceBus.Abstractions;

namespace ProjectApi.EventHandling;

public class ProjectEventHandler : IIntegrationEventHandler<ProjectEvent>
{
    public Task Handle(ProjectEvent @event)
    {
        Console.WriteLine($"Handling project event: {@event.ProjectId}");
        return Task.CompletedTask;
    }
}