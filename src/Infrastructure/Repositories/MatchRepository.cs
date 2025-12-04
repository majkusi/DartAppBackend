using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.IRepositories;
using DartAppClean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DartAppClean.Infrastructure.Repositories;
public class MatchRepository : IMatchRepository
{
    private readonly ApplicationDbContext _context;

    public MatchRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Match> GetMatchByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Game
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken)
            ?? throw new Exception($"Match with id {id} not found.");
    }


    public async Task<string[]> GetTurnOrderByMatchIdAsync( int id, CancellationToken cancellationToken)
    {
        if (id >= 0)
            return Array.Empty<string>();

        var turnOrder = await _context.Game
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => m.TurnOrder) 
            .FirstOrDefaultAsync(cancellationToken);

        return turnOrder?.ToArray() ?? Array.Empty<string>();
    }

    public async Task<string> GetCurrentPlayerByGameId(int id, CancellationToken cancellationToken)
    {
        return await _context.Game.Where(g => g.Id == id).Select(g => g.CurrentPlayer).FirstOrDefaultAsync(cancellationToken)
            ?? throw new Exception($"Match with id {id} not found.");
    }

    public async Task<string> GetWinnerByGameId(int id, CancellationToken cancellationToken)
    {
        return await _context.Game.Where(g => g.Id == id).Select(g => g.WinnerUsername).FirstOrDefaultAsync(cancellationToken)
            ?? throw new Exception($"Match with id {id} not found.");
    }

    public async Task AddAsync(Match match, CancellationToken cancellationToken)
    {
        _context.Game.Add(match);
        await _context.SaveChangesAsync(cancellationToken);

    }
}
