

namespace DartAppClean.Application.Common.Interfaces;

public interface IMatchStateNotificationHub
{
    public async Task JoinGame(int gameId) { }
    public async Task SendGameStateUpdate(int gameId, CancellationToken cancellationToken) { }

}
