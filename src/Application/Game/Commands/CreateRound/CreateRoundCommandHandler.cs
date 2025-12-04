using DartAppClean.Domain.Entities.MatchEntites;
using DartAppClean.Domain.IRepositories;
namespace DartAppClean.Application.Match.Commands.CreateRound;

public record CreateRoundCommand : IRequest<int>
{
    public int GameId { get; init; }
    public int RoundNumber { get; init; }
    public int Points { get; init; }
    public string PlayerUsername { get; init; } = "";

}

public class CreateRoundCommandHandler : IRequestHandler<CreateRoundCommand, int>
{
    private readonly IRoundRepository _roundRepository;
    public CreateRoundCommandHandler(IRoundRepository roundRepository)
    {
        _roundRepository = roundRepository;
    }

    public async Task<int> Handle(CreateRoundCommand request, CancellationToken cancellationToken)
    {
        var round = Round.Create(
            request.GameId,
            request.RoundNumber,
            request.Points,
            request.PlayerUsername);
        await _roundRepository.AddAsync(round, cancellationToken);
        return round.Id;
    }
}
