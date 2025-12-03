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

    public Task<Match> GetMatchByIdAsync(int id, CancellationToken cancellationToken)
    {
        return _context.Game.Where(m => m.Id == id).FirstAsync(cancellationToken);
    }
}
