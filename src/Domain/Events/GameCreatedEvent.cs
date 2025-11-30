using DartAppClean.Domain.Entities.MatchEntites;
namespace DartAppClean.Domain.Events;
public class MatchCreatedEvent : BaseEvent
{
    public MatchCreatedEvent(Match Match)
    {
        this.Match = Match;
    }
    public Match Match { get; }
}
