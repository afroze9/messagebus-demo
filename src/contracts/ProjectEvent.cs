using ServiceBus.Events;

namespace Contracts;

public record ProjectEvent : IntegrationEvent
{
    public int ProjectId { get; init; }

    public ProjectEvent(int projectId) => ProjectId = projectId;
}