using DartAppClean.Domain.Entities.GameEntites;
namespace DartAppClean.Domain.Events;
public class GameCreatedEvent : BaseEvent
{
    public GameCreatedEvent(Game game)
    {
        Game = game;
    }
    public Game Game { get; }
}
