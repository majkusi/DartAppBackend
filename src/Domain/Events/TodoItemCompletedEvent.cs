namespace DartAppClean.Domain.Events;

public class TodoItemCompletedEvent : IBaseEvent
{
    public TodoItemCompletedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}
