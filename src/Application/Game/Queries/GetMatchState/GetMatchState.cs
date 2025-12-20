using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Application.Match.Queries.TeamQueries;

namespace DartAppClean.Application.Match.Queries.GetMatchState;

public record GetMatchStateCommand(int GameId) : IRequest<GetMatchStateResponse>;

public record GetMatchStateResponse(
    int GameId,
    ICollection<string> TurnOrder,
    string? CurrentPlayer,
    IReadOnlyList<TeamsDto> Teams,
    bool Finished,
    string? WinnerUsername
);

public class GetMatchState : IRequestHandler<GetMatchStateCommand, GetMatchStateResponse>
{
    private readonly IMatchReadRepository _matchReadRepository;

    public GetMatchState(
        IMatchReadRepository matchReadRepository
       )
    {
        _matchReadRepository = matchReadRepository;
    }

    public async Task<GetMatchStateResponse> Handle(GetMatchStateCommand request, CancellationToken cancellationToken)
    {
        var gameState = await _matchReadRepository.GetGameStateAsync(request.GameId, cancellationToken);
        var winnerUsername = gameState.Teams
            .SelectMany(t => t.Players)
            .Where(p => p.Winner)
            .Select(p => p.PlayerUsername)
            .FirstOrDefault();

        var winner = !string.IsNullOrEmpty(winnerUsername);

        return new GetMatchStateResponse(
            gameState.GameId,
            gameState.TurnOrder,
            gameState.CurrentPlayer,
            gameState.Teams,
            winner,
            winnerUsername
        );
    }

}
