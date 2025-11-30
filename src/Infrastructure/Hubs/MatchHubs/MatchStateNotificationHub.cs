using DartAppClean.Application.Match.Queries.GetMatchState;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DartAppClean.Application.Hubs.MatchHubs;

public class MatchStateNotificationHub : Hub
{

    private readonly ILogger<MatchStateNotificationHub> _logger;
    private readonly ISender _sender;
    public MatchStateNotificationHub(ILogger<MatchStateNotificationHub> logger, ISender sender)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task JoinGame(int MatchId)
    {
        var groupName = $"Match-{MatchId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        _logger.LogInformation("Player joined {Group}", groupName);

        var matchState = await _sender.Send(new GetMatchStateCommand(MatchId));

        await Clients.Caller.SendAsync("MatchStateUpdated", matchState);
    }
}
