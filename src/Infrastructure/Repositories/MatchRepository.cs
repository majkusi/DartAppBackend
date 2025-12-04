using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.IRepositories;
using DartAppClean.Domain.Services;
using DartAppClean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DartAppClean.Infrastructure.Repositories;
public class MatchRepository : IMatchRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ITurnOrderService _turnOrderService;
    public MatchRepository(ApplicationDbContext context, ITurnOrderService turnOrderService)
    {
        _turnOrderService = turnOrderService;
        _context = context;
    }

    public async Task<Match> GetMatchByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Game
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken)
            ?? throw new Exception($"Match with id {id} not found.");
    }

    public async Task<List<string>> GetTurnOrderByMatchIdAsync(int id, CancellationToken cancellationToken)
    {
        var turnOrder = await _context.Game
            .AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => m.TurnOrder)
            .FirstOrDefaultAsync(cancellationToken);

        return turnOrder?.ToList() ?? new List<string>();
    }

    public async Task<string> GetCurrentPlayerByGameId(int id, CancellationToken cancellationToken)
    {
        return await _context.Game.Where(g => g.Id == id).Select(g => g.CurrentPlayer).FirstOrDefaultAsync(cancellationToken)
            ?? throw new Exception($"Match with id {id} not found.");
    }

    public async Task<string> UpdateCurrentPlayerByGameIdAndUsername(int id, string username, CancellationToken cancellationToken)
    {
        var match = await _context.Game.FirstOrDefaultAsync(g => g.Id == id, cancellationToken);

        if (match == null || match.CurrentPlayer == null)
            return "Match or Current player is null!";
        var next = _turnOrderService.CalculateNextPlayer(match, match.CurrentPlayer);
        match.CurrentPlayer = next;
        await _context.SaveChangesAsync(cancellationToken);
        return next;
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
