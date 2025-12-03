using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.IRepositories;
public interface IMatchRepository
{
    public Task<Match> GetMatchByIdAsync(int id, CancellationToken cancellationToken);

}
