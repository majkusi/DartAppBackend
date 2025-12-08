
using System;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Entities.MatchEntites;
using DartAppClean.Domain.Enums;
using NUnit.Framework;

namespace DartAppClean.Domain.UnitTests
{
    [TestFixture]
    public class TeamPlayerEntityTests
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

        private TeamPlayer CreateTeamPlayer(int initialScore = 50, string username = "alice")
        {
            var team = CreateTeamWithMatch();
            return new TeamPlayer
            {
                Match = team.Match,
                MatchId = team.MatchId,
                Team = team,
                PlayerUsername = username,
                IndividualScore = initialScore,
                Winner = false,
                Order = 1
            };
        }

        // 1) Negative points throw
        [Test]
        public void ScorePoints_ShouldThrow_WhenPointsAreNegative()
        {
            var player = CreateTeamPlayer(initialScore: 50);

            var ex = Assert.Throws<ArgumentException>(() => player.ScorePoints(-1));
            Assert.That(ex!.Message, Is.EqualTo("Points cannot be negative."));
            Assert.That(player.IndividualScore, Is.EqualTo(50));
            Assert.That(player.Winner, Is.False);
        }


        [Test]
        public void ScorePoints_WhenPointsEqualScore_ShouldSetScoreToZero_AndWinnerTrue()
        {
            var player = CreateTeamPlayer(initialScore: 42);

            player.ScorePoints(42);

            Assert.That(player.IndividualScore, Is.EqualTo(0));
            Assert.That(player.Winner, Is.True);
        }


        [Test]
        public void ScorePoints_WhenPointsLessThanScore_ShouldReduceScore_AndWinnerStaysFalse()
        {
            var player = CreateTeamPlayer(initialScore: 60);

            player.ScorePoints(15);

            Assert.That(player.IndividualScore, Is.EqualTo(45));
            Assert.That(player.Winner, Is.False);
        }


        [Test]
        public void ScorePoints_WhenPointsGreaterThanScore_ShouldNotChangeScore_AndWinnerStaysFalse()
        {
            var player = CreateTeamPlayer(initialScore: 30);

            player.ScorePoints(31);

            Assert.That(player.IndividualScore, Is.EqualTo(30)); 
            Assert.That(player.Winner, Is.False);
        }

        
        [Test]
        public void ScorePoints_MultipleCalls_ShouldOnlySetWinnerOnExactFinish()
        {
            var player = CreateTeamPlayer(initialScore: 50);

            player.ScorePoints(20); 
            Assert.That(player.IndividualScore, Is.EqualTo(30));
            Assert.That(player.Winner, Is.False);

            player.ScorePoints(10); 
            Assert.That(player.IndividualScore, Is.EqualTo(20));
            Assert.That(player.Winner, Is.False);

            player.ScorePoints(25); 
            Assert.That(player.IndividualScore, Is.EqualTo(20));
            Assert.That(player.Winner, Is.False);

            player.ScorePoints(20); 
            Assert.That(player.IndividualScore, Is.EqualTo(0));
            Assert.That(player.Winner, Is.True);
        }

        
        [Test]
        public void ScorePoints_ShouldNotChange_OtherFields()
        {
            var team = CreateTeamWithMatch();
            var player = new TeamPlayer
            {
                Match = team.Match,
                MatchId = team.MatchId,
                Team = team,
                PlayerUsername = "bob",
                IndividualScore = 10,
                Winner = false,
                Order = 1
            };

            player.ScorePoints(5);

            Assert.That(player.PlayerUsername, Is.EqualTo("bob"));
            Assert.That(player.Order, Is.EqualTo(1));
            Assert.That(player.Team, Is.SameAs(team));
            Assert.That(player.Match, Is.SameAs(team.Match));
        }

        [Test]
        public void ScorePoints_WhenZeroPoints_ShouldNotChangeScoreOrWinner()
        {
            var player = CreateTeamPlayer(initialScore: 33);

            player.ScorePoints(0);

            Assert.That(player.IndividualScore, Is.EqualTo(33));
            Assert.That(player.Winner, Is.False);
        }
    }
}
