using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.IRepositories;
public interface IRoundRepository
{
    public Task AddAsync(Round round, CancellationToken cancellationToken);
}
