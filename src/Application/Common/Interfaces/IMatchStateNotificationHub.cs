

namespace DartAppClean.Application.Common.Interfaces;

public interface IMatchStateNotificationHub
{
    Task JoinGame(int MatchId);
    Task SendMatchStateUpdate(int MatchId, CancellationToken cancellationToken);
}
