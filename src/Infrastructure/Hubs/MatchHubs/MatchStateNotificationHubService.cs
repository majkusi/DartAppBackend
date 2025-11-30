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

    public async Task SendMatchStateUpdate(int MatchId, CancellationToken cancellationToken)
    {
        var groupName = $"Match-{MatchId}";
        var matchState = await _sender.Send(new GetMatchStateCommand(MatchId), cancellationToken);
        _logger.LogInformation("Sending Match state update to group {Group}", groupName);
        await _hubContext.Clients.Group(groupName).SendAsync("MatchStateUpdated", matchState, cancellationToken);
    }

    public async Task JoinGame(int MatchId)
    {
        await Task.CompletedTask;
    }
}
