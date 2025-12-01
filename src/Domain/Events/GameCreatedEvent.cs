using DartAppClean.Domain.Entities.GameEntites;
namespace DartAppClean.Domain.Events;
public class MatchCreatedEvent : BaseEvent
{
    public MatchCreatedEvent(Match Game)
    {
        this.Game = Game;
    }
    public Match Game { get; }
}
