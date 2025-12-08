using DartAppClean.Domain.Entities.MatchEntites;
using DartAppClean.Domain.IRepositories;
using DartAppClean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DartAppClean.Infrastructure.Repositories;
public class TeamPlayerRepository : ITeamPlayerRepository
{
    private readonly ApplicationDbContext _context;

    public TeamPlayerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TeamPlayer> GetTeamPlayerByUsername(string username, CancellationToken cancellationToken)
    {
        return await _context.TeamPlayer
            .FirstOrDefaultAsync(tp => tp.PlayerUsername == username, cancellationToken)
            ?? throw new Exception($"TeamPlayer with username {username} not found.");
    }
    public async Task<TeamPlayer> GetTeamPlayerByUsernameAndGameId(string username, int gameId, CancellationToken cancellationToken)
    {
        return await _context.TeamPlayer
           .FirstOrDefaultAsync(tp => tp.PlayerUsername == username && tp.MatchId == gameId, cancellationToken)
           ?? throw new Exception($"TeamPlayer with username {username} and/or gameId {gameId} not found.");
    }
    
   
}
