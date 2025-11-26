using DartAppClean.Application.Match.Queries.GetMatchState;
using Microsoft.AspNetCore.SignalR;

namespace DartAppClean.Application.Hubs.MatchHubs;
public class MatchStateNotificationHub : Hub
{

    public async Task SendGameStateUpdate(GetMatchState matchState, CancellationToken cancellationToken)
    {
        await Clients.All.SendAsync("", matchState, cancellationToken);
    }
}
