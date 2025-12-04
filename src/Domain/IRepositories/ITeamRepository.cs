using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartAppClean.Domain.Entities.MatchEntites;

namespace DartAppClean.Domain.IRepositories;

public interface ITeamRepository
{
    Task<List<Team>> GetTeamsByGameIdAsync(int gameId, CancellationToken cancellationToken);
}
