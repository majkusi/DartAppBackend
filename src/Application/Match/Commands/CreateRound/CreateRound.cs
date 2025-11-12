using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Events;
namespace DartAppClean.Application.Match.Commands.CreateRound;


public record CreateRoundCommand : IRequest<int>
{
    public int GameId { get; init; }
    public int RoundNumber { get; init; }
    public int Points { get; init; }
    public int PlayerId { get; init; }
}

public class CreateRound : IRequestHandler<CreateRoundCommand,int>
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
            PlayerId = request.PlayerId
        };
        entity.AddDomainEvent(new RoundCreatedEvent(entity));
        _context.Round.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }

}
