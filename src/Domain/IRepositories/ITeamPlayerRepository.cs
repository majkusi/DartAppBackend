using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.IRepositories;
public interface ITeamPlayerRepository
{
    public Task<TeamPlayer?> GetTeamPlayerByUsername(string username, CancellationToken cancellationToken);

    public Task<TeamPlayer?> GetTeamPlayerByUsernameAndGameId(string username, int gameId, CancellationToken cancellationToken);
}
