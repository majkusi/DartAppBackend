namespace DartAppClean.Domain.Events;

public class TodoItemDeletedEvent : IBaseEvent
{
    public TodoItemDeletedEvent(TodoItem item)
    {
        Item = item;
    }

    public TodoItem Item { get; }
}
