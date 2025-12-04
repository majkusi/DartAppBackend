using DartAppClean.Application.Game.Queries;

namespace DartAppClean.Application.Match.Queries.TeamQueries;
public class TeamsDto
{
    public TeamsDto(int id, int teamNumber, int gameId, IEnumerable<TeamPlayerDto> players)
    {
        Id = id;
        TeamNumber = teamNumber;
        MatchId = gameId;
        Players = players.ToList();
    }
    public int Id { get; init; }
    public int TeamNumber { get; init; }
    public int MatchId { get; init; }
    public int? Score { get; init; }
    public List<TeamPlayerDto> Players { get; init; }
}
