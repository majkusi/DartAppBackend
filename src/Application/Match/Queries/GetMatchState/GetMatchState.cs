using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Application.Match.Queries.TeamQueries;

namespace DartAppClean.Application.Match.Queries.GetMatchState;

public record GetMatchStateCommand(int MatchId) : IRequest<GetMatchStateResponse>;

public record GetMatchStateResponse(
    int MatchId,
    string[] TurnOrder,
    string? CurrentPlayer,
    TeamsDto[] Teams,
    bool MatchFinished,
    string? WinnerUsername);


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
            .Where(t => t.MatchId == request.MatchId)
            .AsNoTracking()
            .ProjectTo<TeamsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);


        var turnOrder = Enumerable.Range(0, teams.Max(t => t.Players.Count()))
            .SelectMany(i => teams
                .Select(t => t.Players.ElementAtOrDefault(i)?.PlayerUsername)
                .Where(username => username != null))
            .ToList();

        var Match = await _context.Match
            .AsNoTracking()
            .FirstOrDefaultAsync(g => g.Id == request.MatchId, cancellationToken);

        var currentPlayer = Match?.CurrentPlayer;
        var finished = Match != null ? Match.MatchFinished : false;
        string? nextPlayer = null;

        if (!string.IsNullOrWhiteSpace(currentPlayer) && turnOrder.Contains(currentPlayer))
        {
            nextPlayer = currentPlayer;
        }
        else
        {
            nextPlayer = turnOrder.FirstOrDefault();
        }

        return new GetMatchStateResponse(
            request.MatchId,
            turnOrder.Cast<string>().ToArray(),
            nextPlayer,
            teams.ToArray(),
            finished,
            Match?.WinnerPlayer
        );
    }
}
