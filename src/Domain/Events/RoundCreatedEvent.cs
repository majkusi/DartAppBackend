namespace DartAppClean.Domain.Events;
public sealed record RoundCreatedEvent(int gameId, int roundId, string playerUsername, int points) : IBaseEvent
{
}
