using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.Events;
public  class TeamCreatedEvent : BaseEvent
{
    public TeamCreatedEvent(Team team)
    {
        Team = team;
    }

    public Team Team { get; }
}
