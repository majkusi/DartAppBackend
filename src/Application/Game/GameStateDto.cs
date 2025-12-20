using DartAppClean.Application.Match.Queries.TeamQueries;

namespace DartAppClean.Application.Game;


public sealed class GameStateDto
{
    public int GameId { get; init; }
    public ICollection<string> TurnOrder { get; init; } = [];
    public string? CurrentPlayer { get; init; }
    public string? WinnerUsername { get; init; }
    public bool Winner => !string.IsNullOrEmpty(WinnerUsername);
    public IReadOnlyList<TeamsDto> Teams { get; init; } = [];
}

