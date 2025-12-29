
using DartAppClean.Application.Common.Interfaces;
using DartAppClean.Application.Match.MatchEventHandlers;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.Events;
using DartAppClean.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DartAppClean.Application.UnitTests.Match.Events
{
    [TestFixture]
    public class RoundCreatedEventHandlerTests
    {
        private Mock<ILogger<RoundCreatedEventHandler>> _loggerMock;
        private Mock<IMatchRepository> _matchRepoMock;
        private Mock<ITeamPlayerRepository> _teamPlayerRepoMock;
        private Mock<IMatchStateNotificationHub> _hubMock;
        private RoundCreatedEventHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<RoundCreatedEventHandler>>();
            _matchRepoMock = new Mock<IMatchRepository>(MockBehavior.Strict);
            _teamPlayerRepoMock = new Mock<ITeamPlayerRepository>(MockBehavior.Strict);
            _hubMock = new Mock<IMatchStateNotificationHub>(MockBehavior.Strict);

            _handler = new RoundCreatedEventHandler(
                _loggerMock.Object,
                _matchRepoMock.Object,
                _teamPlayerRepoMock.Object,
                _hubMock.Object
            );
        }

        private static Domain.Entities.GameEntites.Match CreateTestMatch(int id = 1)
        {
            return new Domain.Entities.GameEntites.Match { Id = id, CurrentPlayer = String.Empty };
        }

        private static TeamPlayer CreateTestPlayer(string username, Domain.Entities.GameEntites.Match match, bool winner = false)
        {
            return new TeamPlayer { PlayerUsername = username, Winner = winner, Match = match };
        }

        [Test]
        public async Task Handle_Should_LogWarning_When_Game_Not_Found()
        {
            var evt = new RoundCreatedEvent(1, "alice", 50);

            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.GameEntites.Match?)null);

            var cts = new CancellationTokenSource();
            await _handler.Handle(evt, cts.Token);

            _matchRepoMock.Verify(r => r.GetMatchByIdAsync(1, cts.Token), Times.Once);

            _teamPlayerRepoMock.VerifyNoOtherCalls();
            _hubMock.VerifyNoOtherCalls();
        }


        [Test]
        public async Task Handle_Should_LogWarning_When_TeamPlayer_Not_Found()
        {
            var evt = new RoundCreatedEvent(1, "alice", 50);

            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(CreateTestMatch());

            _teamPlayerRepoMock
                .Setup(r => r.GetTeamPlayerByUsernameAndGameId("alice", 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((TeamPlayer?)null);

            var cts = new CancellationTokenSource();
            await _handler.Handle(evt, cts.Token);

            _matchRepoMock.VerifyAll();
            _teamPlayerRepoMock.VerifyAll();
            _hubMock.VerifyNoOtherCalls();
        }


        [Test]
        public async Task Handle_Should_Update_Score_Change_Player_And_Send_HubUpdate()
        {
            var evt = new RoundCreatedEvent(1, "alice", 30);

            var match = CreateTestMatch();
            var player = CreateTestPlayer("alice", match);

            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(match);

            _teamPlayerRepoMock
                .Setup(r => r.GetTeamPlayerByUsernameAndGameId("alice", 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(player);

            _matchRepoMock
                .Setup(r => r.UpdateCurrentPlayerByGameIdAndUsername(1, "alice", It.IsAny<CancellationToken>()))
                .ReturnsAsync("bob");

            _hubMock
                .Setup(h => h.SendGameStateUpdate(1, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var cts = new CancellationTokenSource();
            await _handler.Handle(evt, cts.Token);

            Assert.That(match.CurrentPlayer, Is.EqualTo("alice"));
            _matchRepoMock.VerifyAll();
            _teamPlayerRepoMock.VerifyAll();
            _hubMock.VerifyAll();
        }

        [Test]
        public async Task Handle_Should_FinishMatch_When_Player_Is_Winner()
        {
            var evt = new RoundCreatedEvent(1, "alice", 10);

            var match = CreateTestMatch();
            var winnerPlayer = CreateTestPlayer("alice", match, winner: true);

            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(match);

            _teamPlayerRepoMock
                .Setup(r => r.GetTeamPlayerByUsernameAndGameId("alice", 1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(winnerPlayer);

            _matchRepoMock
                .Setup(r => r.UpdateCurrentPlayerByGameIdAndUsername(1, "alice", It.IsAny<CancellationToken>()))
                .ReturnsAsync("bob");

            _hubMock
                .Setup(h => h.SendGameStateUpdate(1, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var cts = new CancellationTokenSource();
            await _handler.Handle(evt, cts.Token);

            Assert.That(winnerPlayer.Winner, Is.True);
            Assert.That(match.WinnerUsername, Is.EqualTo("alice"));
        }

        [Test]
        public void Handle_Should_Propagate_Concurrency_Exception()
        {
            var evt = new RoundCreatedEvent(1, "alice", 10);

            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(1, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            var cts = new CancellationTokenSource();

            Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
                _handler.Handle(evt, cts.Token));
        }

        [Test]
        public void Handle_Should_Propagate_DbUpdateException()
        {
            var evt = new RoundCreatedEvent(1, "alice", 10);

            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(1, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException());

            var cts = new CancellationTokenSource();

            Assert.ThrowsAsync<DbUpdateException>(() =>
                _handler.Handle(evt, cts.Token));
        }
        [Test]
        public void Handle_Should_Throw_When_Cancellation_Requested()
        {
            var evt = new RoundCreatedEvent(1, "alice", 10);

            var cts = new CancellationTokenSource();
            cts.Cancel();

            _matchRepoMock
                .Setup(r => r.GetMatchByIdAsync(It.IsAny<int>(), cts.Token))
                .ThrowsAsync(new TaskCanceledException());

            Assert.ThrowsAsync<TaskCanceledException>(() =>
                _handler.Handle(evt, cts.Token));
        }

    }
}
