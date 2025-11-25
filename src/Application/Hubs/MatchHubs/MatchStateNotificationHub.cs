using DartAppClean.Domain.Entities.GameEntites;
using Microsoft.AspNetCore.SignalR;

namespace DartAppClean.Application.Hubs.MatchHubs;
public class MatchStateNotificationHub : Hub
{

    public async Task SendGameStateUpdate(Game game, CancellationToken cancellationToken)
    {
        await Clients.All.SendAsync("", game, cancellationToken);
    }
}
