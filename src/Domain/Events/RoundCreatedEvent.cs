namespace DartAppClean.Domain.Events;
public sealed record RoundCreatedEvent(int gameId, string playerUsername, int points) : IBaseEvent
{
}
