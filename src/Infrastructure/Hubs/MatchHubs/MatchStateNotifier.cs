
using DartAppClean.Application.Hubs.MatchHubs;
using DartAppClean.Application.Match.Queries.GetMatchState;
using Microsoft.AspNetCore.SignalR;

public class MatchStateNotifier
{
    private readonly IHubContext<MatchStateNotificationHub> _hubContext;

    public MatchStateNotifier(IHubContext<MatchStateNotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task BroadcastAsync(GetMatchStateResponse matchState, CancellationToken ct)
    {
        var groupName = $"game-{matchState.GameId}";
        await _hubContext.Clients.Group(groupName).SendAsync("GameStateUpdated", matchState, ct);
    }
}
