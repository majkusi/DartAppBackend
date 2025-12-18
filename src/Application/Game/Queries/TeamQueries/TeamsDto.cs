using DartAppClean.Application.Game.Queries;

namespace DartAppClean.Application.Match.Queries.TeamQueries;


public class TeamsDto
{
    public TeamsDto() { }
    public TeamsDto(int id, int teamNumber, int matchId, IEnumerable<TeamPlayerX01Dto> players)
    {
        Id = id;
        TeamNumber = teamNumber;
        MatchId = matchId;
        Players = players.ToList();
    }
    public int Id { get; init; }
    public int TeamNumber { get; init; }
    public int MatchId { get; init; }
    public int? Score { get; init; }
    public List<TeamPlayerX01Dto> Players { get; init; } = new();
}
