using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Application.Match.Queries.TeamQueries;
using DartAppClean.Domain.Entities.MatchEntites;
using DartAppClean.Domain.IRepositories;
using DartAppClean.Domain.Services;

namespace DartAppClean.Application.Match.Queries.GetMatchState;

public record GetMatchStateCommand(int GameId) : IRequest<GetMatchStateResponse>;


public record GetMatchStateResponse(
    int GameId,
    IReadOnlyList<string> TurnOrder,
    string? CurrentPlayer,
    IReadOnlyList<TeamsDto> Teams,
    bool Finished,
    string? WinnerUsername,
    string? NextPlayer
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
        var teams = _teamRepository.GetTeamsByGameIdAsync(request.GameId, cancellationToken);
        var turnOrder = _matchRepository.GetTurnOrderByMatchId(request.GameId, cancellationToken);

        var currentPlayer = _matchRepository.GetCurrentPlayerByGameId(request.GameId, cancellationToken);

        if(currentPlayer == null) throw new Exception("currentPlayer is null!");
        var nextPlayer = _turnOrderService.CalculateNextPlayer(currentPlayer).ToString();

        var winnerUsername = _matchRepository.GetWinnerByGameId(request.GameId, cancellationToken).ToString();
        bool winner = false;
        if (!String.IsNullOrEmpty(winnerUsername))
        {
            winner = true;
        }
        return new GetMatchStateResponse(
            request.GameId,
            turnOrder,
            currentPlayer,
            teams,
            winner,
            winnerUsername,
            nextPlayer
        );
    }
}
