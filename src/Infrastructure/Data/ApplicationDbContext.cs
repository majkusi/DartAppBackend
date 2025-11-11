using System.Reflection;
using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Entities;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DartAppClean.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TodoList> TodoLists => Set<TodoList>();

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public DbSet<Game> Game => Set<Game>();
    public DbSet<Team> Team => Set<Team>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Entity<Game>(e =>
        {
            e.ToTable("Games", "game");
            e.HasKey(g => g.Id);

            e.HasMany(g => g.Teams)
                .WithOne(t => t.Game)
                .HasForeignKey(t => t.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(g => g.Rounds)
                .WithOne(r => r.Game)
                .HasForeignKey(r => r.GameId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Team>(e =>
        {
            e.ToTable("Teams", "game");
            e.HasKey(t => t.Id);

            e.HasMany(t => t.Players)
                .WithOne(tp => tp.Team)
                .HasForeignKey(tp => tp.TeamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TeamPlayer>(e =>
        {
            e.ToTable("TeamPlayers", "game");
            e.HasKey(tp => tp.Id);          
        });

        builder.Entity<Round>(e =>
        {
            e.ToTable("Rounds", "game");
            e.HasKey(r => r.Id);

            e.HasMany(r => r.Scores)
                .WithOne(rs => rs.Round)
                .HasForeignKey(rs => rs.RoundId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<RoundScore>(e =>
        {
            e.ToTable("RoundScores", "game");
            e.HasKey(rs => rs.Id);

            e.HasOne(rs => rs.TeamPlayer)
                .WithMany()
                .HasForeignKey(rs => rs.TeamPlayerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    
    }
}
