using DartAppClean.Application.Match.Commands.CreateRound;
using DartAppClean.Domain.Entities.GameEntites;
using DartAppClean.Domain.IRepositories;
using Moq;
using NUnit.Framework;

namespace DartAppClean.Application.UnitTests.Match.Commands
{
    [TestFixture]
    public class CreateRoundCommandHandlerTests
    {
        private Mock<IRoundRepository> _repoMock = default!;
        private CreateRoundCommandHandler _handler = default!;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IRoundRepository>(MockBehavior.Strict);
            _handler = new CreateRoundCommandHandler(_repoMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _repoMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task Handle_Should_Create_Save_And_Return_Id()
        {
            var cmd = new CreateRoundCommand
            {
                MatchId = 42,
                RoundNumber = 3,
                Points = 140,
                PlayerUsername = "alice"
            };

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<Round>(), It.IsAny<CancellationToken>()))
                .Callback<Round, CancellationToken>((round, _) => round.Id = 101)
                .Returns(Task.CompletedTask);

            var cts = new CancellationTokenSource();

            var id = await _handler.Handle(cmd, cts.Token);

            Assert.That(id, Is.EqualTo(101));
            _repoMock.Verify(r => r.AddAsync(
                It.Is<Round>(round =>
                    round.MatchId == 42 &&
                    round.RoundNumber == 3 &&
                    round.Points == 140 &&
                    round.PlayerUsername == "alice"
                ),
                cts.Token),
                Times.Once);
        }

        [Test]
        public void Handle_Should_Propagate_Domain_Exception_For_Invalid_Input()
        {
            var cmd = new CreateRoundCommand
            {
                MatchId = 42,
                RoundNumber = 0,
                Points = -10,
                PlayerUsername = ""
            };

            var cts = new CancellationTokenSource();

            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(cmd, cts.Token));

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Round>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void Handle_Should_Propagate_When_Repository_Throws()
        {
            var cmd = new CreateRoundCommand
            {
                MatchId = 42,
                RoundNumber = 1,
                Points = 60,
                PlayerUsername = "bob"
            };

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<Round>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB down"));

            var cts = new CancellationTokenSource();

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _handler.Handle(cmd, cts.Token));

            Assert.That(ex!.Message, Does.Contain("DB down"));
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Round>(), cts.Token), Times.Once);
        }

        [Test]
        public async Task Handle_Should_Pass_CancellationToken_To_Repository()
        {
            var cmd = new CreateRoundCommand
            {
                MatchId = 99,
                RoundNumber = 2,
                Points = 100,
                PlayerUsername = "carol"
            };

            var cts = new CancellationTokenSource();

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<Round>(), It.IsAny<CancellationToken>()))
                .Callback<Round, CancellationToken>((round, _) => round.Id = 777)
                .Returns(Task.CompletedTask);

            var id = await _handler.Handle(cmd, cts.Token);

            Assert.That(id, Is.EqualTo(777));
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Round>(), cts.Token), Times.Once);
        }
    }
}
