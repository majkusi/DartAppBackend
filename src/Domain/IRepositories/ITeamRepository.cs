using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.IRepositories;

public interface ITeamRepository
{
    Task<List<Team>> GetTeamsByGameIdAsync(int gameId, CancellationToken cancellationToken);
}
