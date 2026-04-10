
using Xunit;
using Moq;
using SpaceBattle.lib;
using App.Scopes;
using App;

public class MoveCommandTests
{
    [Fact]
    public void MoveCorrectTest()
    {
        var moving = new Mock<IMovingObject>();
        moving.SetupGet(a => a.Position).Returns(new Vector([12, 5]));
        moving.SetupGet(a => a.Velocity).Returns(new Vector([-4, 1]));

        SpaceBattle.lib.ICommand cmd = new MoveCommand(moving.Object);
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

public class RegisterIoCDependencyMoveCommandTest
{
    public RegisterIoCDependencyMoveCommandTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void General()
    {
        var moving = new Mock<IMovingObject>();
        moving.SetupGet(m => m.Position).Returns(new Vector([12, 5]));
        moving.SetupGet(m => m.Velocity).Returns(new Vector([-4, 1]));

        Ioc.Resolve<App.ICommand>(
            "IoC.Register", 
            "Adapters.IMovingObject", 
            (object[] args) => moving.Object
        ).Execute();

        new RegisterIoCDependencyMoveCommand().Execute();
        var move = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Move", new object());
        
        move.Execute();

        moving.VerifySet(m => m.Position = new Vector([8, 6]), Times.Once);
    }
}
