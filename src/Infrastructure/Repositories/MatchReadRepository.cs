using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Application.Game;
using DartAppClean.Application.Game.Queries;
using DartAppClean.Application.Match.Queries.TeamQueries;
using Microsoft.EntityFrameworkCore;

namespace DartAppClean.Infrastructure.Repositories;

public sealed class MatchReadRepository : IMatchReadRepository
{
    private readonly IApplicationDbContext _context;

    public MatchReadRepository(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<GameStateDto> GetGameStateAsync(int matchId, CancellationToken cancellationToken)
    {
        var match = await _context.Game
            .AsNoTracking()
            .AsSplitQuery()
            .Where(g => g.Id == matchId)
            .Select(g => new GameStateDto
            {
                GameId = g.Id,
                TurnOrder = g.TurnOrder,
                CurrentPlayer = g.CurrentPlayer,

                Teams = g.Teams
                .OrderBy(t => t.TeamNumber)
                .Select(t => new TeamsDto
                {
                    Id = t.Id,
                    TeamNumber = t.TeamNumber,
                    MatchId = t.MatchId,
                    Score = t.Score,
                    Players = t.Players
                        .OrderBy(p => p.Order)
                        .Select(p => new TeamPlayerDto
                        {
                            PlayerUsername = p.PlayerUsername,
                            IndividualScore = p.IndividualScore,
                            Winner = p.Winner,
                            Order = p.Order
                        })
                        .ToList()
                })
                .ToList()

            })
            .SingleOrDefaultAsync(cancellationToken);
        if (match == null)
        {
            throw new Exception("Match is null");
        }
        return match;
    }
}
