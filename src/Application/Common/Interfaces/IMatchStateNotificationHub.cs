

namespace DartAppClean.Application.Common.Interfaces;

public interface IMatchStateNotificationHub
{
    Task JoinGame(int gameId);
    Task SendGameStateUpdate(int gameId, CancellationToken cancellationToken);

}
