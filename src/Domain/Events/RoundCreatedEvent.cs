using DartAppClean.Domain.Entities.GameEntites;
namespace DartAppClean.Domain.Events;
public class RoundCreatedEvent : BaseEvent
{
    public RoundCreatedEvent(Round round)
    {
        Round = round;
    }
    public Round Round { get; }
}
