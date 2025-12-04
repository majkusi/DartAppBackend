using DartAppClean.Application.Game.Queries;
using DartAppClean.Application.Match.Queries.TeamQueries;
using DartAppClean.Domain.IRepositories;
using DartAppClean.Domain.Services;

namespace DartAppClean.Application.Match.Queries.GetMatchState;

public record GetMatchStateCommand(int GameId) : IRequest<GetMatchStateResponse>;

public record GetMatchStateResponse(
    int GameId,
    List<string> TurnOrder,
    string? CurrentPlayer,
    List<TeamsDto> Teams,
    bool Finished,
    string? WinnerUsername
);

public class GetMatchState : IRequestHandler<GetMatchStateCommand, GetMatchStateResponse>
{
    private readonly ITeamRepository _teamRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly ITurnOrderService _turnOrderService;
    public GetMatchState(
        ITeamRepository teamRepository,
        IMatchRepository matchRepository,
        ITurnOrderService turnOrderService)
    {
        _turnOrderService = turnOrderService;
        _matchRepository = matchRepository;
        _teamRepository = teamRepository;
    }
    public async Task<GetMatchStateResponse> Handle(GetMatchStateCommand request, CancellationToken cancellationToken)
    {
        var turnOrder = await _matchRepository.GetTurnOrderByMatchIdAsync(request.GameId, cancellationToken);
        string currentPlayer = await _matchRepository.GetCurrentPlayerByGameId(request.GameId, cancellationToken);
        var match = await _matchRepository.GetMatchByIdAsync(request.GameId, cancellationToken);

        string winnerUsername = await _matchRepository.GetWinnerByGameId(request.GameId, cancellationToken);
        bool winner = !string.IsNullOrEmpty(winnerUsername);

        var teams = await _teamRepository.GetTeamsByGameIdAsync(request.GameId, cancellationToken);
        var teamsDto = teams.Select(t =>
            new TeamsDto(
                t.Id,
                t.TeamNumber,
                t.GameId,
                t.Players
                    .OrderBy(p => p.Order)
                    .Select(p => new TeamPlayerDto
                    {
                        PlayerUsername = p.PlayerUsername,
                        IndividualScore = p.IndividualScore,
                        Winner = p.Winner,
                        Order = p.Order
                    })
            )).ToList();

        return new GetMatchStateResponse(
            request.GameId,
            turnOrder,
            currentPlayer,
            teamsDto,
            winner,
            winnerUsername
        );
    }
}
