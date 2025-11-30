using DartAppClean.Domain.Entities;
using DartAppClean.Domain.Entities.MatchEntites;

namespace DartAppClean.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }
    
    DbSet<Domain.Entities.MatchEntites.Match> Match { get; }
    DbSet<Team> Team { get; }
    DbSet<Round> Round { get; }
    DbSet<TeamPlayer> TeamPlayer { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
