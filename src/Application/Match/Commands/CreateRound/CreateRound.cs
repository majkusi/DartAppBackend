using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Entities.MatchEntites;
using DartAppClean.Domain.Events;
namespace DartAppClean.Application.Match.Commands.CreateRound;

public record CreateRoundCommand : IRequest<int>
{
    public int MatchId { get; init; }
    public int RoundNumber { get; init; }
    public int Points { get; init; }
    public string PlayerUsername { get; init; } = "";

}

public class CreateRound : IRequestHandler<CreateRoundCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMatchStateNotificationHub _notificationHub;
    public CreateRound(IApplicationDbContext context, IMatchStateNotificationHub notificationHub)
    {
        _notificationHub = notificationHub;
        _context = context;
    }

    public async Task<int> Handle(CreateRoundCommand request, CancellationToken cancellationToken)
    {
        var entity = new Round
        {
            MatchId = request.MatchId,
            RoundNumber = request.RoundNumber,
            Points = request.Points,
            PlayerUsername = request.PlayerUsername
        };

        entity.AddDomainEvent(new RoundCreatedEvent(entity));
        _context.Round.Add(entity);

        var teamPlayer = _context.TeamPlayer
            .FirstOrDefault(tp => tp.MatchId == request.MatchId && tp.PlayerUsername == request.PlayerUsername);

        if (teamPlayer != null)
        {
            teamPlayer.ScorePoints(request.Points);
        }



        var teams = await _context.Team
            .Where(t => t.MatchId == request.MatchId)
            .Include(t => t.Players.OrderBy(p => p.Id))
            .ToListAsync(cancellationToken);



        var maxPlayers = teams.Max(t => t.Players.Count);
        var turnOrder = Enumerable.Range(0, maxPlayers)
            .SelectMany(i => teams
                .Select(t => t.Players.ElementAtOrDefault(i)?.PlayerUsername)
                .Where(username => username is not null))
            .Cast<string>()
            .ToList();
        var currentIndex = turnOrder.IndexOf(request.PlayerUsername);
        var nextPlayer = turnOrder[(currentIndex + 1) % turnOrder.Count];

        var Match = await _context.Match.FirstAsync(g => g.Id == request.MatchId, cancellationToken);
        Match.CurrentPlayer = nextPlayer;

        await _context.SaveChangesAsync(cancellationToken);

        await _notificationHub.SendMatchStateUpdate(request.MatchId, cancellationToken);
        await _notificationHub.JoinGame(request.MatchId);
        return entity.RoundNumber;
    }
}
