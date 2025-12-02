namespace DartAppClean.Domain.Events;

public class TodoItemCreatedEvent : IBaseEvent
{
    public TodoItemCreatedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}
