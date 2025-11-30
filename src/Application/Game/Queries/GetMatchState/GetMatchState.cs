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
            .AsNoTracking()
            .ProjectTo<TeamsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);


        var teamPlayer = _context.TeamPlayer
            .Where(g => g.GameId == request.GameId)
            .FirstOrDefault();


        var turnOrder = Enumerable.Range(0, teams.Max(t => t.Players.Count()))
            .SelectMany(i => teams
                .Select(t => t.Players.ElementAtOrDefault(i)?.PlayerUsername)
                .Where(username => username != null))
            .ToList();

        var game = await _context.Game
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == request.GameId, cancellationToken);

        var persistedCurrent = game?.CurrentPlayer;

        string? effectiveCurrentPlayer = null;

        if (!string.IsNullOrWhiteSpace(persistedCurrent) && turnOrder.Contains(persistedCurrent))
        {
            effectiveCurrentPlayer = persistedCurrent;
        }
        else
        {
            effectiveCurrentPlayer = turnOrder.FirstOrDefault();
        }
        bool winner;
        string winnerUsername;

        return new GetMatchStateResponse(
            request.GameId,
            turnOrder.Cast<string>().ToArray(),
            effectiveCurrentPlayer,
            teams.ToArray(),
            winner = (teamPlayer != null && teamPlayer.Winner == true) ? true : false,
            winnerUsername = (teamPlayer != null && teamPlayer.Winner == true) ? teamPlayer.PlayerUsername : String.Empty
        );
    }
}
