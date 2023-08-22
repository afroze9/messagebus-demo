using ServiceBus.Events;

namespace Contracts;

public record TodoEvent : IntegrationEvent
{
    public int TodoId { get; init; }

    public TodoEvent(int todoId) => TodoId = todoId;
}