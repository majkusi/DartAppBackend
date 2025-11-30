using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Entities.MatchEntites;
using DartAppClean.Domain.Enums;
using DartAppClean.Domain.Events;

namespace DartAppClean.Application.Match.Commands.CreateMatch;

public record CreateMatchCommand : IRequest<int>
{
    public MatchTypesEnum MatchType { get; init; }
    public X01TypeEnum? X01TypeEnum { get; init; }
    public List<string> PlayersName { get; init; } = new List<string>();
    public bool TeamsMode { get; init; }
    public int Score { get; set; }
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
        var MatchEntity = new Domain.Entities.MatchEntites.Match
        {
            MatchTypes = request.MatchType,
            X01TypeEnum = request.X01TypeEnum ?? null
        };
        MatchEntity.AssignTeams(request.PlayersName, request.TeamsMode, request.Score);
        MatchEntity.AddDomainEvent(new MatchCreatedEvent(MatchEntity));
        _context.Match.Add(MatchEntity);
        await _context.SaveChangesAsync(cancellationToken);

        return MatchEntity.Id;
    }
}
