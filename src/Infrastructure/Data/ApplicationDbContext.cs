using System.Reflection;
using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Entities;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Entities.MatchEntites;
using DartAppClean.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DartAppClean.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<Match> Game => Set<Match>();
    public DbSet<Team> Team => Set<Team>();
    public DbSet<Round> Round => Set<Round>();
    public DbSet<TeamPlayer> TeamPlayer => Set<TeamPlayer>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Entity<Match>(e =>
        {
            e.ToTable("Matchs", "Match");
            e.HasKey(g => g.Id);

            e.HasMany(g => g.Teams)
                .WithOne(t => t.Match)
                .HasForeignKey(t => t.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(g => g.Rounds)
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Team>(e =>
        {
            e.ToTable("Teams", "Match");
            e.HasKey(t => t.Id);

            e.HasMany(t => t.Players)
                .WithOne(tp => tp.Team)
                .HasForeignKey(tp => tp.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TeamPlayer>(e =>
        {
            e.ToTable("TeamPlayers", "Match");
            e.HasKey(tp => tp.Id);
        });

        builder.Entity<Round>(e =>
        {
            e.ToTable("Rounds", "Match");
            e.HasKey(r => r.Id);
        });
    }
}
