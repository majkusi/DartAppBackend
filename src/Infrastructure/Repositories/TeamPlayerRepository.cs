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

    public Task<TeamPlayer> GetTeamPlayerByUsername(string username, CancellationToken cancellationToken)
    {
        return _context.TeamPlayer.Where(tp => tp.PlayerUsername == username).FirstAsync(cancellationToken);
    }
    public Task<TeamPlayer> GetTeamPlayerByUsernameAndGameId(string username, int gameId, CancellationToken cancellationToken)
    {
        return _context.TeamPlayer.Where(tp => tp.PlayerUsername == username && tp.GameId == gameId).FirstAsync(cancellationToken);
    }

}
