using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Enums;
using NUnit.Framework;

namespace DartAppClean.Domain.UnitTests.EntityTests
{
    [TestFixture]
    public class MatchEntityTests
    {
        [Test]
        public void Create_ShouldInitializeProperties_AndRaiseDomainEvent()
        {
            var gameType = GameTypesEnum.X01;
            X01TypeEnum x01Type = X01TypeEnum.DIDO;
            var match = Match.Create(gameType, x01Type);
            Assert.That(match.GameTypes, Is.EqualTo(gameType));
            Assert.That(match.X01TypeEnum, Is.EqualTo(x01Type));
            Assert.That(match.GameFinished, Is.False);
            Assert.That(match.WinnerUsername, Is.Empty);
        }

        [Test]
        public void FinishMatch_ShouldSetGameFinished_AndWinnerUsername()
        {
            var match = Match.Create(GameTypesEnum.X01, X01TypeEnum.DISO);

            match.FinishMatch("alice");

            Assert.That(match.GameFinished, Is.True);
            Assert.That(match.WinnerUsername, Is.EqualTo("alice"));
        }

        [Test]
        public void AssignTeams_TeamsMode_RequiresEvenNumberOfPlayers_ThrowsOnOdd()
        {
            var players = new List<string> { "alice", "bob", "carol" };
            var match = Match.Create(GameTypesEnum.X01, X01TypeEnum.SIDO);
            const int score = 501;

            var ex = Assert.Throws<ArgumentException>(() => match.AssignTeams(players, teamsMode: true, score));
            Assert.That(ex!.Message, Does.Contain("Teams mode requires pairs"));
        }

        [Test]
        public void AssignTeams_TeamsMode_PairedTeams_TurnOrder_P1sThenP2s()
        {
            var players = new List<string> { "alice", "bob", "carol", "dave" };
            var match = Match.Create(GameTypesEnum.X01, X01TypeEnum.SIDO);
            const int score = 501;

            match.AssignTeams(players, teamsMode: true, score);

            Assert.That(match.Teams.Count, Is.EqualTo(2));

            var team1 = match.Teams.Single(t => t.TeamNumber == 1);
            var team2 = match.Teams.Single(t => t.TeamNumber == 2);

            Assert.That(team1.Players.Count, Is.EqualTo(2));
            var t1p1 = team1.Players.Single(p => p.PlayerUsername == "alice");
            var t1p2 = team1.Players.Single(p => p.PlayerUsername == "bob");
            Assert.That(t1p1.Order, Is.EqualTo(1));
            Assert.That(t1p2.Order, Is.EqualTo(2));

            Assert.That(team2.Players.Count, Is.EqualTo(2));
            var t2p1 = team2.Players.Single(p => p.PlayerUsername == "carol");
            var t2p2 = team2.Players.Single(p => p.PlayerUsername == "dave");
            Assert.That(t2p1.Order, Is.EqualTo(1));
            Assert.That(t2p2.Order, Is.EqualTo(2));

            Assert.That(team1.Score, Is.EqualTo(score));
            Assert.That(team2.Score, Is.EqualTo(score));

            var expectedTurnOrder = new[] { "alice", "carol", "bob", "dave" };
            Assert.That(match.TurnOrder, Is.EqualTo(expectedTurnOrder));
            Assert.That(match.CurrentPlayer, Is.EqualTo("carol"));
        }

        [Test]
        public void AssignTeams_SoloMode_SinglePlayerTeams_TurnOrder_CurrentPlayer()
        {
            var players = new List<string> { "alice", "bob", "carol" };
            var match = Match.Create(GameTypesEnum.CRICKET, X01TypeEnum.SISO);
            int score = 0;
            bool teamsMode = false;

            match.AssignTeams(players, teamsMode, score);

            Assert.That(match.Teams.Count, Is.EqualTo(3));
            foreach (var (team, idx) in match.Teams.Select((t, i) => (t, i)))
            {
                Assert.That(team.TeamNumber, Is.EqualTo(idx + 1));
                Assert.That(team.Players.Count, Is.EqualTo(1));
                Assert.That(team.Players.Single().PlayerUsername, Is.EqualTo(players[idx]));
            }

            Assert.That(match.TurnOrder, Is.EqualTo(players));

            Assert.That(match.CurrentPlayer, Is.EqualTo("alice"));
        }

        [Test]
        public void AssignTeams_ShouldThrow_WhenPlayersIsNull()
        {
            var match = Match.Create(GameTypesEnum.X01, X01TypeEnum.SISO);

            Assert.Throws<ArgumentNullException>(() =>
            {
                match.AssignTeams(null!, teamsMode: false, score: 0);
            });
        }

        [Test]
        public void AssignTeams_ShouldThrow_WhenPlayersIsEmpty()
        {
            var match = Match.Create(GameTypesEnum.X01, X01TypeEnum.SISO);
            Assert.Throws<ArgumentException>(() =>
            {
                match.AssignTeams(new List<string>(), teamsMode: false, score: 0);
            });
        }
    }
}
