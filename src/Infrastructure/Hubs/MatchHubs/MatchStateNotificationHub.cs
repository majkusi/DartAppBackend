using DartAppClean.Application.Match.Queries.GetMatchState;
using Microsoft.AspNetCore.SignalR;

namespace DartAppClean.Application.Hubs.MatchHubs;

public class MatchStateNotificationHub : Hub
{
    public async Task JoinGame(int gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"game-{gameId}");
    }

    public async Task SendGameStateUpdate(GetMatchStateResponse matchState, CancellationToken cancellationToken)
    {
        await Clients.Group($"game-{matchState.GameId}")
                     .SendAsync("GameStateUpdated", matchState, cancellationToken);
    }
}

