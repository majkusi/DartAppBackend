
using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Application.Hubs.MatchHubs;
using DartAppClean.Application.Match.Queries.GetMatchState;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

public class MatchStateNotificationHubService : IMatchStateNotificationHub
{
    private readonly IHubContext<MatchStateNotificationHub> _hubContext;
    private readonly ISender _sender;
    private readonly ILogger<MatchStateNotificationHubService> _logger;

    public MatchStateNotificationHubService(
        IHubContext<MatchStateNotificationHub> hubContext,
        ISender sender,
        ILogger<MatchStateNotificationHubService> logger)
    {
        _hubContext = hubContext;
        _sender = sender;
        _logger = logger;
    }

    public async Task SendGameStateUpdate(int gameId, CancellationToken cancellationToken)
    {
        var groupName = $"game-{gameId}";
        var matchState = await _sender.Send(new GetMatchStateCommand(gameId), cancellationToken);

        _logger.LogInformation("Sending game state update to group {Group}", groupName);
        await _hubContext.Clients.Group(groupName).SendAsync("GameStateUpdated", matchState, cancellationToken);
    }

    public Task JoinGame(int gameId)
    {
        return Task.CompletedTask;
    }
}
