using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Events;
using DartAppClean.Domain.IRepositories;
using Microsoft.Extensions.Logging;

namespace DartAppClean.Application.Match.MatchEventHandlers;

public class RoundCreatedEventHandler : INotificationHandler<RoundCreatedEvent>
{
    private readonly ILogger<RoundCreatedEventHandler> _logger;
    private readonly IMatchRepository _matchRepository;
    private readonly ITeamPlayerRepository _teamPlayerRepository;
    private readonly IMatchStateNotificationHub _hub;

    public RoundCreatedEventHandler(
        ILogger<RoundCreatedEventHandler> logger,
        IMatchRepository matchRepository,
        ITeamPlayerRepository teamPlayerRepository,
        IMatchStateNotificationHub hub)
    {
        _logger = logger;
        _matchRepository = matchRepository;
        _teamPlayerRepository = teamPlayerRepository;
        _hub = hub;
    }

    public async Task Handle(RoundCreatedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var game = await _matchRepository.GetMatchByIdAsync(notification.gameId, cancellationToken);
            if (game == null)
            {
                _logger.LogWarning("RoundCreatedEventHandler: game with id {GameId} not found", notification.gameId);
                return;
            }

            var teamPlayer = await _teamPlayerRepository.GetTeamPlayerByUsernameAndGameId(
                notification.playerUsername, notification.gameId, cancellationToken);
            if (teamPlayer == null)
            {
                _logger.LogWarning("RoundCreatedEventHandler: team player {Player} not found in game {GameId}",
                    notification.playerUsername, notification.gameId);
                return;
            }

            teamPlayer.ScorePoints(notification.points);
            game.CurrentPlayer = notification.playerUsername;

            if (teamPlayer.Winner)
            {
                game.FinishMatch(notification.playerUsername);
            }

            string nextPlayer = await _matchRepository.UpdateCurrentPlayerByGameIdAndUsername(
                notification.gameId, notification.playerUsername, cancellationToken);

            await _hub.SendGameStateUpdate(notification.gameId, cancellationToken);

            _logger.LogInformation("Processed RoundCreatedEvent: GameId={GameId}, Player={Player}, Points={Points}",
                notification.gameId, notification.playerUsername, notification.points);
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
