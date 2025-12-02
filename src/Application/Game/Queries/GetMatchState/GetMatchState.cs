using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Application.Match.Queries.TeamQueries;

namespace DartAppClean.Application.Match.Queries.GetMatchState;

public record GetMatchStateCommand(int GameId) : IRequest<GetMatchStateResponse>;

public record GetMatchStateResponse(
    int GameId,
    string[] TurnOrder,
    string? CurrentPlayer,
    TeamsDto[] Teams,
    bool Finished,
    string WinnerUsername);


public class GetMatchState : IRequestHandler<GetMatchStateCommand, GetMatchStateResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetMatchState(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetMatchStateResponse> Handle(GetMatchStateCommand request, CancellationToken cancellationToken)
    {
        var teams = await _context.Team
            .Where(t => t.GameId == request.GameId)
            .ProjectTo<TeamsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var teamPlayer = _context.TeamPlayer
            .Where(g => g.GameId == request.GameId)
            .FirstOrDefault();

        var game = await _context.Game
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);


        if (game == null) throw new Exception("Game is null!");
        var turnOrder = game.TurnOrder.ToArray();

        var currentPlayer = game?.CurrentPlayer;

        var nextPlayer = string.Empty;

        if (!string.IsNullOrWhiteSpace(currentPlayer) && turnOrder.Contains(currentPlayer))
        {
            int currentPlayerIndex = Array.IndexOf(turnOrder, currentPlayer);
            nextPlayer = turnOrder[(currentPlayerIndex + 1) % turnOrder.Length];
        }
        else
        {
            nextPlayer = turnOrder.FirstOrDefault();
        }


        bool winner = (teamPlayer != null && teamPlayer.Winner == true) ? true : false;
        string winnerUsername = (teamPlayer != null && teamPlayer.Winner == true) ? teamPlayer.PlayerUsername : String.Empty;

        return new GetMatchStateResponse(
            request.GameId,
            turnOrder,
            nextPlayer,
            teams.ToArray(),
            winner,
            winnerUsername
        );
    }
}
