using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Application.Match.Queries.TeamQueries;

namespace DartAppClean.Application.Match.Queries.GetMatchState;

public record GetMatchStateCommand(int GameId, string PlayerUsername) : IRequest<GetMatchStateResponse>;
public record GetMatchStateResponse(
    int GameId,
    string[] TurnOrder,
    string? CurrentPlayer,
    TeamsDto[] Teams);

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
            .Where(g => g.GameId == request.GameId)
            .AsNoTracking()
            .ProjectTo<TeamsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var maxPlayers = teams.Max(t => t.Players.Count());
        var turnOrder = new List<string>();

        for (int i = 0; i < maxPlayers; i++)
        {
            foreach (var team in teams)
            {
                var players = team.Players.ToList();
                turnOrder.Add(players[i].PlayerUsername);
            }
        }

        var currentPlayer = turnOrder.IndexOf(request.PlayerUsername);
        var nextPlayer = turnOrder[(currentPlayer + 1) % turnOrder.Count];

        return new GetMatchStateResponse(
            request.GameId,
            turnOrder.ToArray(),
            nextPlayer,
            teams.ToArray()
        );
    }

}
