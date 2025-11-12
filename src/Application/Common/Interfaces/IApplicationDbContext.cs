using DartAppClean.Domain.Entities;
using DartAppClean.Domain.Entities.GameEntites;

namespace DartAppClean.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Game> Game { get; }
    DbSet<Team> Team { get; }
    DbSet<Round> Round { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
