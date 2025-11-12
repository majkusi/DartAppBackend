using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Enums;
using DartAppClean.Domain.Events;

namespace DartAppClean.Application.Match.Commands.CreateMatch;

public record CreateMatchCommand : IRequest<int>
{
    public GameTypesEnum GameType { get; init; } 
    public X01TypeEnum? X01TypeEnum { get; init; }
}

public class CreateMatch : IRequestHandler<CreateMatchCommand, int>
{

    private readonly IApplicationDbContext _context;

    public CreateMatch(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
    {

        var entity = new Game
        {
            GameTypes = request.GameType,
            X01TypeEnum = request.X01TypeEnum ?? null
        };
        entity.AddDomainEvent(new GameCreatedEvent(entity));
        _context.Game.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
