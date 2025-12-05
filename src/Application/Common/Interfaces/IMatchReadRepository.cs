using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Application.Game;

namespace DartAppClean.Application.Common.Interfaces;

public interface IMatchReadRepository
{
    public Task<GameStateDto> GetGameStateAsync(int matchId, CancellationToken cancellationToken);

}
