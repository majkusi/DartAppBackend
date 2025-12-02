using DartAppClean.Domain.Entities.MatchEntites;

namespace DartAppClean.Domain.Events;
public class TeamCreatedEvent : IBaseEvent
{
    public TeamCreatedEvent(Team team)
    {
        Team = team;
    }

    public Team Team { get; }
}
