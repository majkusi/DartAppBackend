using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Domain.Services;

public class TurnOrderService : ITurnOrderService
{
    private readonly Match _match;

    public TurnOrderService(Match match)
    {
        _match = match; 
    }

    public async Task<string> CalculateNextPlayer(string currentPlayer)
    {
        
        List<string> turnOrder = _match.TurnOrder;
        if (turnOrder == null) return "Turn Order is null or empty";
        int indexOfCurrentPlayer = turnOrder.IndexOf(currentPlayer); 
        string nextPlayer = turnOrder[indexOfCurrentPlayer+1];
        return nextPlayer ?? "";
    }
}
