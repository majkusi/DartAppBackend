using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.IRepositories;
using DartAppClean.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DartAppClean.Infrastructure.Repositories;
public class RoundRepository : IRoundRepository
{
    private readonly ApplicationDbContext _context;

    public RoundRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Round> GetRoundByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Round
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken)
            ?? throw new Exception($"Round with id {id} not found.");
    }
    public async Task AddAsync(Round round, CancellationToken cancellationToken)
    {
        _context.Round.Add(round);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
