
using System.Threading;
using System.Threading.Tasks;
using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Domain.Entities;
using DartAppClean.Domain.Entities.GameEntites;
using Microsoft.EntityFrameworkCore;

namespace Application.FunctionalTests.TestingInfrastructure
{
    public class ApplicationDbContextTest : DbContext, IApplicationDbContext
    {
        public ApplicationDbContextTest(DbContextOptions<ApplicationDbContextTest> options)
            : base(options) { }


        public DbSet<TodoList> TodoLists => Set<TodoList>();
        public DbSet<TodoItem> TodoItems => Set<TodoItem>();

        public DbSet<DartAppClean.Domain.Entities.GameEntites.Match> Game => Set<DartAppClean.Domain.Entities.GameEntites.Match>();
        public DbSet<Team> Team => Set<Team>();
        public DbSet<Round> Round => Set<Round>();
        public DbSet<TeamPlayer> TeamPlayer => Set<TeamPlayer>();

        public new Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
            base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DartAppClean.Domain.Entities.GameEntites.Match>(b =>
            {
                b.HasKey(m => m.Id);


                b.Property(m => m.TurnOrder).HasColumnType("text[]");

                b.HasMany(m => m.Teams)
                 .WithOne(t => t.Match)            
                 .HasForeignKey(t => t.MatchId);
            });

            modelBuilder.Entity<Team>(b =>
            {
                b.HasKey(t => t.Id);
                b.Property(t => t.TeamNumber);
                b.Property(t => t.Score);

   
                b.HasMany(t => t.Players)
                 .WithOne(p => p.Team)             
                 .HasForeignKey(p => p.TeamId);
            });

            modelBuilder.Entity<TeamPlayer>(b =>
            {
                b.HasKey(p => p.Id);
                b.Property(p => p.PlayerUsername)
                 .IsRequired();
                b.Property(p => p.IndividualScore);
                b.Property(p => p.Winner);
                b.Property(p => p.Order);
            });

            modelBuilder.Entity<Round>(b =>
            {
                b.HasKey(r => r.Id);
                b.HasOne<DartAppClean.Domain.Entities.GameEntites.Match>().WithMany().HasForeignKey(r => r.MatchId);
            });
        }
    }
}
