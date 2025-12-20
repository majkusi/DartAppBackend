using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Services;

public class TurnOrderService : ITurnOrderService
{
    public string CalculateNextPlayer(Match match, string currentPlayer)
    {
        var list = match.TurnOrder as IList<string>;
        if (match.TurnOrder == null || match.TurnOrder.Count == 0 || list is null)
            return "Turn Order is null or empty";
        int indexOfCurrentPlayer = list.IndexOf(currentPlayer);

        int nextIndex = (indexOfCurrentPlayer + 1) % match.TurnOrder.Count;

        return list[nextIndex];
    }
}
