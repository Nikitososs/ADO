
using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;
using Xunit;

public class RotateTests
{
    [Fact]
    public void Rotate_ShouldChangeAngle_From45To90()
    {
        var rotatable = new Mock<IRotatableObject>();
        rotatable.SetupProperty(r => r.Angle, new Angle(1));
        rotatable.SetupGet(r => r.AngularVelocity).Returns(new Angle(1));
        var command = new RotateCommand(rotatable.Object);

        command.Execute();

        Assert.Equal(new Angle(2), rotatable.Object.Angle);
    }

    [Fact]
    public void Rotate_ShouldThrowException_WhenAngleCannotBeRead()
    {
        var rotatable = new Mock<IRotatableObject>();
        rotatable.SetupGet(r => r.Angle).Throws<Exception>();
        rotatable.SetupGet(r => r.AngularVelocity).Returns(new Angle(1));
        var command = new RotateCommand(rotatable.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void Rotate_ShouldThrowException_WhenVelocityCannotBeRead()
    {
        var rotatable = new Mock<IRotatableObject>();
        rotatable.SetupGet(r => r.Angle).Returns(new Angle(1));
        rotatable.SetupGet(r => r.AngularVelocity).Throws<Exception>();
        var command = new RotateCommand(rotatable.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }

    [Fact]
    public void Rotate_ShouldThrowException_WhenAngleCannotBeSet()
    {
        var rotatable = new Mock<IRotatableObject>();
        rotatable.SetupGet(r => r.Angle).Returns(new Angle(1));
        rotatable.SetupGet(r => r.AngularVelocity).Returns(new Angle(1));
        rotatable.SetupSet(r => r.Angle = It.IsAny<Angle>()).Throws<Exception>();

        var command = new RotateCommand(rotatable.Object);

        Assert.Throws<Exception>(() => command.Execute());
    }
}

public class RegisterIoCDependencyRotateCommandTest
{
    public RegisterIoCDependencyRotateCommandTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void RegisterRotate_ShouldAllowResolvingAndExecutingCommand()
    {
        var rotating = new Mock<IRotatableObject>();
        rotating.SetupProperty(r => r.Angle, new Angle(1));
        rotating.SetupGet(r => r.AngularVelocity).Returns(new Angle(1));

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Adapters.IRotatingObject",
            (object[] args) => rotating.Object
        ).Execute();

        new RegisterIoCDependencyRotateCommand().Execute();

        var rotateCmd = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Rotate", new object());

        rotateCmd.Execute();

        Assert.Equal(new Angle(2), rotating.Object.Angle);
        rotating.VerifySet(r => r.Angle = new Angle(2), Times.Once);
    }
}

