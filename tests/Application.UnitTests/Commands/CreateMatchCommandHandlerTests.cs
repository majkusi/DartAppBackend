
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DartAppClean.Application.Match.Commands.CreateMatch;
using DartAppClean.Domain.Enums;
using DartAppClean.Domain.IRepositories;
using DartAppClean.Domain.Entities.GameEntites;
using Moq;
using NUnit.Framework;
using MatchEntity = DartAppClean.Domain.Entities.GameEntites.Match;

namespace DartAppClean.Application.UnitTests.Match.Commands
{
    [TestFixture]
    public class CreateMatchCommandHandlerTests
    {
        private Mock<IMatchRepository> _repoMock;
        private CreateMatchCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _repoMock = new Mock<IMatchRepository>(MockBehavior.Strict);
            _handler = new CreateMatchCommandHandler(_repoMock.Object);
        }

        [Test]
        public async Task Handle_Should_Create_Assign_Save_And_Return_Id()
        {

            var cmd = new CreateMatchCommand
            {
                GameType = GameTypesEnum.X01,
                X01TypeEnum = X01TypeEnum.SIDO,
                PlayersName = new List<string> { "alice", "bob" },
                TeamsMode = true,
                Score = 501
            };


            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<MatchEntity>(), It.IsAny<CancellationToken>()))
                .Callback<MatchEntity, CancellationToken>((m, ct) => m.Id = 123)
                .Returns(Task.CompletedTask);

            var cts = new CancellationTokenSource();
            var id = await _handler.Handle(cmd, cts.Token);
            Assert.That(id, Is.EqualTo(123));
            _repoMock.Verify(r => r.AddAsync(It.Is<MatchEntity>(m =>
                    m.GameTypes == GameTypesEnum.X01 &&
                    m.X01TypeEnum == X01TypeEnum.SIDO
                ), cts.Token),
                Times.Once);
        }

        [Test]
        public void Handle_Should_Propagate_Domain_Exception_For_Invalid_Players()
        {
            var cmd = new CreateMatchCommand
            {
                GameType = GameTypesEnum.X01,
                X01TypeEnum = X01TypeEnum.SIDO,
                PlayersName = new List<string> { "alice", "bob", "carol" },
                TeamsMode = true,
                Score = 501
            };

            var cts = new CancellationTokenSource();

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _handler.Handle(cmd, cts.Token));

            Assert.That(ex!.Message, Does.Contain("Teams mode")); 
            _repoMock.Verify(r => r.AddAsync(It.IsAny<MatchEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public void Handle_Should_Propagate_When_Repository_Throws()
        {
            var cmd = new CreateMatchCommand
            {
                GameType = GameTypesEnum.X01,
                X01TypeEnum = X01TypeEnum.SIDO,
                PlayersName = new List<string> { "alice", "bob" },
                TeamsMode = true,
                Score = 501
            };

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<MatchEntity>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB down"));

            var cts = new CancellationTokenSource();

            var ex = Assert.ThrowsAsync<Exception>(async () =>
                await _handler.Handle(cmd, cts.Token));

            Assert.That(ex!.Message, Does.Contain("DB down"));
            _repoMock.Verify(r => r.AddAsync(It.IsAny<MatchEntity>(), cts.Token), Times.Once);
        }

        [Test]
        public async Task Handle_Should_Pass_CancellationToken_To_Repository()
        {
            var cmd = new CreateMatchCommand
            {
                GameType = GameTypesEnum.CRICKET,
                X01TypeEnum = null,
                PlayersName = new List<string> { "alice" },
                TeamsMode = false,
                Score = 0
            };

            var cts = new CancellationTokenSource();

            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<MatchEntity>(), It.IsAny<CancellationToken>()))
                .Callback<MatchEntity, CancellationToken>((m, ct) => m.Id = 777)
                .Returns(Task.CompletedTask);

            var id = await _handler.Handle(cmd, cts.Token);

            Assert.That(id, Is.EqualTo(777));
            _repoMock.Verify(r => r.AddAsync(It.IsAny<MatchEntity>(), cts.Token), Times.Once);
        }
    }
}
