
using System;
using System.Linq;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Entities.MatchEntites;
using DartAppClean.Domain.Enums;
using NUnit.Framework;

namespace DartAppClean.Domain.UnitTests
{
    [TestFixture]
    public class TeamEntityTests
    {
        private Match CreateMatch() => Match.Create(GameTypesEnum.X01, X01TypeEnum.SIDO);

        private Team CreateTeamWithMatch()
        {
            var match = CreateMatch();
            var matchId = match.Id != 0 ? match.Id : 123;

            return new Team
            {
                Match = match,
                MatchId = matchId,
                TeamNumber = 1,
                Score = 501
            };
        }

        [Test]
        public void AddPlayer_ShouldThrow_WhenMatchIsNull()
        {
            var match = CreateMatch();
            var team = new Team
            {
                Match = null!,
                MatchId = match.Id != 0 ? match.Id : 123,
                TeamNumber = 1,
                Score = 0
            };

            var ex = Assert.Throws<InvalidOperationException>(() =>
                team.AddPlayer("alice", score: 0, matchId: team.MatchId));

            Assert.That(ex!.Message, Is.EqualTo("Team.Match must be set before adding players."));
        }

        [Test]
        public void AddPlayer_ShouldThrow_WhenProvidedMatchId_DoesNotMatch_TeamMatchId()
        {
            var team = CreateTeamWithMatch();
            var wrongMatchId = team.MatchId + 1;

            var ex = Assert.Throws<ArgumentException>(() =>
                team.AddPlayer("alice", score: 10, matchId: wrongMatchId));

            Assert.That(ex!.Message, Does.Contain("Provided MatchId does not match Team.MatchId"));
        }

        [Test]
        public void AddPlayer_ShouldThrow_WhenUsernameIsNullOrWhitespace_AfterTrim()
        {
            var team = CreateTeamWithMatch();

            Assert.Throws<ArgumentException>(() =>
                team.AddPlayer(null!, score: 0, matchId: team.MatchId));

            Assert.Throws<ArgumentException>(() =>
                team.AddPlayer("", score: 0, matchId: team.MatchId));

            var ex = Assert.Throws<ArgumentException>(() =>
                team.AddPlayer("   ", score: 0, matchId: team.MatchId));

            Assert.That(ex!.ParamName, Is.EqualTo("username"));
            Assert.That(ex!.Message, Does.Contain("Player username cannot be empty or whitespace"));
        }

        [Test]
        public void AddPlayer_ShouldAddTeamPlayer_WithCorrectFields_AndReferences()
        {
            var team = CreateTeamWithMatch();

            team.AddPlayer("  alice  ", score: 42, matchId: team.MatchId);

            Assert.That(team.Players.Count, Is.EqualTo(1));

            var tp = team.Players.Single();
            Assert.That(tp.PlayerUsername, Is.EqualTo("alice"));
            Assert.That(tp.IndividualScore, Is.EqualTo(42));
            Assert.That(tp.Team, Is.SameAs(team));
            Assert.That(tp.Match, Is.SameAs(team.Match));
        }

        private static readonly string[] expected = new[] { "alice", "bob" };

        [Test]
        public void AddPlayer_MultipleCalls_ShouldAccumulatePlayers()
        {
            var team = CreateTeamWithMatch();

            team.AddPlayer("alice", score: 10, matchId: team.MatchId);
            team.AddPlayer("bob", score: 20, matchId: team.MatchId);

            Assert.That(team.Players.Count, Is.EqualTo(2));
            Assert.That(
                team.Players.Select(p => p.PlayerUsername).ToArray()
                        , Is.EqualTo(expected).AsCollection);
            Assert.That(
                team.Players.Select(p => p.IndividualScore).ToArray()
                        , Is.EqualTo(new[] { 10, 20 }).AsCollection);

            foreach (var p in team.Players)
            {
                Assert.That(p.Team, Is.SameAs(team));
                Assert.That(p.Match, Is.SameAs(team.Match));
            }
        }

        [Test]
        public void Team_Defaults_ShouldInitializePlayersCollection()
        {
            var team = new Team
            {
                Match = CreateMatch(),
                MatchId = 777,
                TeamNumber = 2,
                Score = 0
            };

            Assert.That(team.Players, Is.Not.Null);
            Assert.That(team.Players.Count, Is.EqualTo(0));
        }
    }
}
