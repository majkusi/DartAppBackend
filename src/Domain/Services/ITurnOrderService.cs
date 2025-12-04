using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.Services;

public interface ITurnOrderService
{
    public string CalculateNextPlayer(Match match, string currentPlayer);
}

