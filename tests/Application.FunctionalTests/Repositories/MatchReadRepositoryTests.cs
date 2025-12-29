using Application.FunctionalTests.TestingInfrastructure;
using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Application.FunctionalTests.Repositories;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
namespace Application.FunctionalTests.Repositories
{
    [TestFixture]
    public class MatchReadRepositoryTests : RepositoryTestBase
    {

        private async Task SeedMatchAsync(DbContextOptions<ApplicationDbContextTest> options, int matchId)
        {
            using var testContext = new ApplicationDbContextTest(options);

            var match = new DartAppClean.Domain.Entities.GameEntites.Match
            {
                Id = matchId,
                GameTypes = null,
                X01TypeEnum = null,
                CricketTypeEnum = null,
                CurrentPlayer = "alice",
                WinnerUsername = string.Empty,
                GameFinished = false,
                TurnOrder = new List<string> { "alice", "bob" },
            };

            var team1 = new Team
            {
                Match = match,
                MatchId = matchId,
                TeamNumber = 1,
                Score = 300,
            };

            team1.AddPlayer("alice", score: 100, matchId: matchId);
            var alice = team1.Players.First(p => p.PlayerUsername == "alice");
            alice.Order = 1;

            team1.AddPlayer("dave", score: 80, matchId: matchId);
            var dave = team1.Players.First(p => p.PlayerUsername == "dave");
            dave.Order = 2;

            var team2 = new Team
            {
                Match = match,
                MatchId = matchId,
                TeamNumber = 2,
                Score = 200,
            };

            team2.AddPlayer("bob", score: 50, matchId: matchId);
            var bob = team2.Players.First(p => p.PlayerUsername == "bob");
            bob.Order = 1;

            team2.AddPlayer("carol", score: 75, matchId: matchId);
            var carol = team2.Players.First(p => p.PlayerUsername == "carol");
            carol.Order = 2;

            match.Teams = new List<Team> { team1, team2 };

            testContext.Game.Add(match);
            await testContext.SaveChangesAsync();
        }

        [Test]
        public async Task GetGameStateAsync_Returns_State_With_Correct_Ordering()
        {
            var matchId = 501;
            await SeedMatchAsync(_options, matchId);

            using var ctx = new ApplicationDbContextTest(_options);
            IApplicationDbContext appCtx = ctx;
            var repo = new MatchReadRepository(appCtx);

            var state = await repo.GetGameStateAsync(matchId, CancellationToken.None);

            Assert.That(state, Is.Not.Null);
            Assert.That(state.GameId, Is.EqualTo(matchId));
            Assert.That(state.TurnOrder, Is.EqualTo(new[] { "alice", "bob" }));
            Assert.That(state.CurrentPlayer, Is.EqualTo("alice"));

            Assert.That(state.Teams.Select(t => t.TeamNumber), Is.EqualTo(new[] { 1, 2 }));

            Assert.That(state.Teams[0].Players.Select(p => p.PlayerUsername), Is.EqualTo(new[] { "alice", "dave" }));
            Assert.That(state.Teams[1].Players.Select(p => p.PlayerUsername), Is.EqualTo(new[] { "bob", "carol" }));
        }

        [Test]
        public void GetGameStateAsync_Throws_When_NotFound()
        {
            using var ctx = new ApplicationDbContextTest(_options);
            IApplicationDbContext appCtx = ctx;
            var repo = new MatchReadRepository(appCtx);

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await repo.GetGameStateAsync(9999, CancellationToken.None));

            Assert.That(ex!.Message, Does.Contain("Match is null"));
        }

        [Test]
        public void GetGameStateAsync_Respects_CancellationToken()
        {
            using var ctx = new ApplicationDbContextTest(_options);
            IApplicationDbContext appCtx = ctx;
            var repo = new MatchReadRepository(appCtx);

            using var cts = new CancellationTokenSource();
            cts.Cancel();

            Assert.ThrowsAsync<TaskCanceledException>(async () =>
                await repo.GetGameStateAsync(501, cts.Token));
        }
    }
}
