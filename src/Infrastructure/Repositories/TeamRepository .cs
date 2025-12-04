using DartAppClean.Domain.Entities.MatchEntites;
using DartAppClean.Domain.IRepositories;
using DartAppClean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DartAppClean.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ApplicationDbContext _context;
    public TeamRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Team>> GetTeamsByGameIdAsync(int gameId, CancellationToken cancellationToken)
    {
        return await _context.Team
            .Include(t => t.Players)
            .Where(t => t.GameId == gameId)
            .ToListAsync(cancellationToken);
    }
}
