using DartAppClean.Domain.Entities.GameEntites;
namespace DartAppClean.Domain.Events;
public class MatchCreatedEvent : BaseEvent
{
    public MatchCreatedEvent(Game Game)
    {
        this.Game = Game;
    }
    public Game Game { get; }
}
