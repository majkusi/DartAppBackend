public interface IMatchStateNotificationHub
{
    Task JoinGame(int MatchId);
    Task SendMatchStateUpdate(int MatchId, CancellationToken cancellationToken);
}
