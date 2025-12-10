using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.Events;
public class TeamCreatedEvent : IBaseEvent
{
    public TeamCreatedEvent(Team team)
    {
        Team = team;
    }

    public Team Team { get; }
}
