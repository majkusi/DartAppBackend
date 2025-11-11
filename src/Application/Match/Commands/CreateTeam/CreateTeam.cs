using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Application.Match.GameEventHandlers;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Events;

namespace DartAppClean.Application.Match.Commands.CreateTeam;

public record CreateTeamCommand : IRequest<int>
{
    public int GameId { get; init; }
    public List<string> PlayersName { get; init; } = new List<string>();
    
}


public class CreateTeam :IRequestHandler<CreateTeamCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTeam(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle (CreateTeamCommand request, CancellationToken cancellationToken)
    {
        var teamPlayers = new List<TeamPlayer>(request.PlayersName.Count);
        for(int i=0; i < request.PlayersName.Count; i++)
        {
            teamPlayers.Add(new TeamPlayer
            {
                PlayerUsername = request.PlayersName[i]
            });
        }

        var entity = new Team
        {
            GameId = request.GameId,
            Players = teamPlayers
        };       
        entity.AddDomainEvent(new TeamCreatedEvent(entity));
        _context.Team.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

}
