using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Services;

public class TurnOrderService : ITurnOrderService
{
    public string CalculateNextPlayer(Match match, string currentPlayer)
    {
        if (match.TurnOrder == null || match.TurnOrder.Count == 0)
            return "Turn Order is null or empty";

        int indexOfCurrentPlayer = match.TurnOrder.IndexOf(currentPlayer);

        int nextIndex = (indexOfCurrentPlayer + 1) % match.TurnOrder.Count;

        return match.TurnOrder[nextIndex];
    }
}
