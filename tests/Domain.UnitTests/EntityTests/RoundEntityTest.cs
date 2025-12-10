using DartAppClean.Domain.Entities.GameEntites;
using NUnit.Framework;

namespace DartAppClean.Domain.UnitTests.EntityTests;
[TestFixture]
public class RoundEntityTest
{

    private Match CreateMatch() => Match.Create(Enums.GameTypesEnum.X01, Enums.X01TypeEnum.SIDO);
    [Test]
    public void Create_ShouldInitializeProperties_AndRaiseDomainEvent()
    {
        var match = CreateMatch();

        var round = Round.Create(
            match.Id,
            1,
            20,
            "player1"
        );
        Assert.NotNull(round);
    }
    [Test]
    public void Create_ShouldThrow_WhenMatchIdIsLessThenZero()
    {
        var ex = Assert.Throws<ArgumentException>(() =>
        Round.Create(
            -1,
            1,
            20,
            "player1")
            );
        Assert.That(ex!.Message, Is.EqualTo("Provided matchId is less to zero"));
    }

    [Test]
    public void Create_ShouldThrow_WhenUsernameIsEmpty()
    {
        var match = CreateMatch();
        var ex = Assert.Throws<ArgumentException>(() =>
            Round.Create(
                match.Id,
                1,
                20,
                ""));
        Assert.That(ex!.Message, Is.EqualTo("Provided playerUsername is null or empty"));
    }

    [Test]
    public void Create_ShouldThrow_WhenUsernameIsNull()
    {
        var match = CreateMatch();
        var ex = Assert.Throws<ArgumentException>(() =>
            Round.Create(
                match.Id,
                1,
                20,
                null!));
        Assert.That(ex!.Message, Is.EqualTo("Provided playerUsername is null or empty"));
    }

    [Test]
    public void Create_ShouldThrow_WhenPointsAreLessThenZero()
    {
        var match = CreateMatch();
        var ex = Assert.Throws<ArgumentException>(() =>
            Round.Create(
                match.Id,
                1,
                -10,
                "player1"));
        Assert.That(ex!.Message, Is.EqualTo("Provided points are less than zero"));
    }
}
