using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.IRepositories;
public interface IMatchRepository
{
    public Task<Match> GetMatchByIdAsync(int id, CancellationToken cancellationToken);
    public Task AddAsync(Match match, CancellationToken cancellationToken);
    public Task<string> GetCurrentPlayerByGameId(int id, CancellationToken cancellationToken);
    public Task<string> UpdateCurrentPlayerByGameIdAndUsername(int id, string username, CancellationToken cancellationToken);
    public Task<List<string>> GetTurnOrderByMatchIdAsync(int id, CancellationToken cancellationToken); public Task<string> GetWinnerByGameId(int id, CancellationToken cancellationToken);
}
