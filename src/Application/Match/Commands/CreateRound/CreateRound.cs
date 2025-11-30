using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Events;
namespace DartAppClean.Application.Match.Commands.CreateRound;

public record CreateRoundCommand : IRequest<int>
{
    public int GameId { get; init; }
    public int RoundNumber { get; init; }
    public int Points { get; init; }
    public string PlayerUsername { get; init; } = "";

}

public class CreateRound : IRequestHandler<CreateRoundCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateRound(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateRoundCommand request, CancellationToken cancellationToken)
    {
        var entity = new Round
        {
            GameId = request.GameId,
            RoundNumber = request.RoundNumber,
            Points = request.Points,
            PlayerUsername = request.PlayerUsername
        };
        entity.AddDomainEvent(new RoundCreatedEvent(entity));
        _context.Round.Add(entity);
        var teamPlayer = _context.TeamPlayer
            .FirstOrDefault(tp =>
             tp.GameId == request.GameId &&
             tp.PlayerUsername == request.PlayerUsername
            );
        if (teamPlayer != null)
        {
            teamPlayer.ScorePoints(request.Points);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return entity.RoundNumber;
    }

}
