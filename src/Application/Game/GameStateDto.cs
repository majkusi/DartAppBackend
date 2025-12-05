using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Application.Match.Queries.TeamQueries;

namespace DartAppClean.Application.Game;


public sealed class GameStateDto
{
    public int GameId { get; init; }
    public IReadOnlyList<string> TurnOrder { get; init; } = [];
    public string? CurrentPlayer { get; init; }
    public string? WinnerUsername { get; init; }
    public bool Winner => !string.IsNullOrEmpty(WinnerUsername);
    public IReadOnlyList<TeamsDto> Teams { get; init; } = [];
}

