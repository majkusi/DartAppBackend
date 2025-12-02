using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Events;
using Microsoft.Extensions.Logging;

namespace DartAppClean.Application.Match.MatchEventHandlers;
public class RoundCreatedEventHandler : INotificationHandler<RoundCreatedEvent>
{
    private readonly ILogger<RoundCreatedEventHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly IMatchStateNotificationHub _hub;
    public RoundCreatedEventHandler(ILogger<RoundCreatedEventHandler> logger, IApplicationDbContext context, IMatchStateNotificationHub hub)
    {
        _logger = logger;
        _context = context;
        _hub = hub;
    }

    public async Task Handle(RoundCreatedEvent notification, CancellationToken cancellationToken)
    {
        var teamPlayer = await _context.TeamPlayer
            .FirstOrDefaultAsync(tp => tp.GameId == notification.gameId && tp.PlayerUsername == notification.playerUsername, cancellationToken);
        if (teamPlayer == null)
        {
            _logger.LogWarning("RoundCreatedEventHandler: team player {Player} not found in game {GameId}",
                notification.playerUsername, notification.gameId);
            return;
        }

        var game = await _context.Game
            .FirstOrDefaultAsync(g => g.Id == notification.gameId, cancellationToken);
        if (game == null)
        {
            _logger.LogWarning("RoundCreatedEventHandler: game with id {GameId} not found", notification.gameId);
            return;
        }

        game.CurrentPlayer = notification.playerUsername;
        teamPlayer.ScorePoints(notification.points);
        if (teamPlayer.Winner)
        {
            game.FinishMatch(notification.playerUsername);
        }

        try
        {
            _logger.LogInformation("Processed RoundCreatedEvent: GameId={GameId}, Player={Player}, Points={Points}",
                notification.gameId, notification.playerUsername, notification.points);
            await _hub.SendGameStateUpdate(notification.gameId, cancellationToken);

        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error while processing RoundCreatedEvent: GameId={GameId}", notification.gameId);
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while processing RoundCreatedEvent: GameId={GameId}", notification.gameId);
            throw;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("RoundCreatedEvent handling cancelled");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while processing RoundCreatedEvent: GameId={GameId}", notification.gameId);
            throw;
        }
    }
}
