using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Entities.MatchEntites;
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
        var round = new Round
        {
            GameId = request.GameId,
            RoundNumber = request.RoundNumber,
            Points = request.Points,
            PlayerUsername = request.PlayerUsername
        };
        _context.Round.Add(round);
        round.AddDomainEvent(new RoundCreatedEvent(request.GameId, round.PlayerUsername, round.Points));
        await _context.SaveChangesAsync(cancellationToken);
        return round.Id;
    }
}
