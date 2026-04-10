
using Xunit;
using Moq;
using SpaceBattle.lib;

public class MoveCommandTests
{
    [Fact]
    public void MoveCorrectTest()
    {
        var moving = new Mock<IMovingObject>();
        moving.SetupGet(a => a.Position).Returns(new Vector([12, 5]));
        moving.SetupGet(a => a.Velocity).Returns(new Vector([-4, 1]));

        ICommand cmd = new MoveCommand(moving.Object);
        cmd.Execute();

        moving.VerifySet(a => a.Position = new Vector([8, 6]));
    }

    [Fact]
    public void CantDefinePositionThrowsException()
    {
        var moving = new Mock<IMovingObject>();
        moving.SetupGet(a => a.Position).Throws<InvalidOperationException>();

        Assert.Throws<InvalidOperationException>(() => new MoveCommand(moving.Object).Execute());
    }

    [Fact]
    public void CantDefineVelocityThrowsException()
    {
        var moving = new Mock<IMovingObject>();
        moving.SetupGet(a => a.Position).Returns(new Vector([12, 5]));
        moving.SetupGet(a => a.Velocity).Throws<InvalidOperationException>();

        Assert.Throws<InvalidOperationException>(() => new MoveCommand(moving.Object).Execute());
    }

    [Fact]
    public void CantSetPositionThrowsException()
    {
        var moving = new Mock<IMovingObject>();
        moving.SetupGet(a => a.Position).Returns(new Vector([12, 5]));
        moving.SetupGet(a => a.Velocity).Returns(new Vector([-4, 1]));
        moving.SetupSet(a => a.Position = new Vector([8, 6])).Throws<InvalidOperationException>();

        Assert.Throws<InvalidOperationException>(() => new MoveCommand(moving.Object).Execute());
    }
}
