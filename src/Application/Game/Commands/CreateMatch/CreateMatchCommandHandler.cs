using DartAppClean.Domain.Enums;
using DartAppClean.Domain.IRepositories;

namespace DartAppClean.Application.Match.Commands.CreateMatch;

public record CreateMatchCommand : IRequest<int>
{
    public GameTypesEnum GameType { get; init; }
    public X01TypeEnum? X01TypeEnum { get; init; }
    public List<string> PlayersName { get; init; } = new List<string>();
    public bool TeamsMode { get; init; }
    public int Score { get; set; }
}

public class CreateMatchCommandHandler : IRequestHandler<CreateMatchCommand, int>
{

    private readonly IMatchRepository _matchRepository;

    public CreateMatchCommandHandler(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }

    public async Task<int> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
    {
        var match = Domain.Entities.GameEntites.Match.Create(request.GameType, request.X01TypeEnum);
        match.AssignTeams(request.PlayersName, request.TeamsMode, request.Score);
        await _matchRepository.AddAsync(match, cancellationToken);
        return match.Id;
    }
}
