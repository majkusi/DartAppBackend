using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartAppClean.Domain.Services;

public interface ITurnOrderService
{
    public Task<string> CalculateNextPlayer(string currentPlayer);
}

