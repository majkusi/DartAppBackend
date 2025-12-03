using DartAppClean.Domain.Entities.MatchEntites;

namespace DartAppClean.Domain.IRepositories;
public interface IRoundRepository
{
    public Task AddAsync(Round round, CancellationToken cancellationToken);
}
