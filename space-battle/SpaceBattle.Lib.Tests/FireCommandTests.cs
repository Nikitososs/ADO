using App;
using App.Scopes;
using Moq;
using System.Collections.Generic;
using SpaceBattle.lib;

public class FireCommandTests
{
    public FireCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_WhenAuthorized_CreatesTorpedoInRegistryAndStartsMove()
    {
        var shooter = new Mock<IShootable>();
        shooter.SetupGet(s => s.Position).Returns(new Vector(0, 0));
        shooter.SetupGet(s => s.Facing).Returns(new Angle(0));

        var authorizer = new Mock<IActionAuthorizer>();
        authorizer.Setup(a => a.CanPerform("user-1", "ship-1", FireCommand.FireAction)).Returns(true);

        var torpedo = new Torpedo("torpedo-1", new Vector(0, 0), new Vector(-5, 12));
        var factory = new Mock<ITorpedoFactory>();
        factory.Setup(f => f.Create(shooter.Object, It.IsAny<Vector>())).Returns(torpedo);

        var repository = new Mock<IGameObjectRepository>();
        var moveOperation = new Mock<SpaceBattle.lib.ICommand>();
        var receiver = new Mock<ICommandReceiver>();

        RegisterFireTestInfrastructure(repository.Object, moveOperation.Object);

        var fire = new FireCommand(
            "user-1",
            "ship-1",
            shooter.Object,
            new Vector(-5, 12),
            authorizer.Object,
            factory.Object,
            receiver.Object);

        fire.Execute();

        repository.Verify(r => r.Add("torpedo-1", torpedo), Times.Once());
        factory.Verify(f => f.Create(shooter.Object, It.Is<Vector>(v => v == new Vector(-5, 12))), Times.Once());
        moveOperation.Verify(m => m.Execute(), Times.Once());
        receiver.Verify(r => r.Receive(It.IsAny<SpaceBattle.lib.ICommand>()), Times.Once());
    }

    [Fact]
    public void Execute_WhenNotAuthorized_ThrowsAndDoesNotCreateTorpedo()
    {
        var authorizer = new Mock<IActionAuthorizer>();
        authorizer.Setup(a => a.CanPerform(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        var factory = new Mock<ITorpedoFactory>();
        var repository = new Mock<IGameObjectRepository>();
        RegisterFireTestInfrastructure(repository.Object, new Mock<SpaceBattle.lib.ICommand>().Object);

        var fire = new FireCommand(
            "user-1",
            "ship-1",
            Mock.Of<IShootable>(),
            new Vector(1, 0),
            authorizer.Object,
            factory.Object,
            Mock.Of<ICommandReceiver>());

        Assert.Throws<UnauthorizedAccessException>(() => fire.Execute());

        factory.Verify(f => f.Create(It.IsAny<IShootable>(), It.IsAny<Vector>()), Times.Never());
        repository.Verify(r => r.Add(It.IsAny<string>(), It.IsAny<object>()), Times.Never());
    }

    [Fact]
    public void RegisterIoCDependencyFireCommand_ResolvesAndExecutesFire()
    {
        var shooter = new Mock<IShootable>();
        shooter.SetupGet(s => s.Position).Returns(new Vector(1, 2));
        shooter.SetupGet(s => s.Facing).Returns(new Angle(1));

        var repository = new Mock<IGameObjectRepository>();
        var moveOperation = new Mock<SpaceBattle.lib.ICommand>();
        var receiver = new Mock<ICommandReceiver>();

        RegisterFireTestInfrastructure(repository.Object, moveOperation.Object);
        new RegisterIoCDependencyActionAuthorizer().Execute();
        var authorizer = (PrefixTreeActionAuthorizer)Ioc.Resolve<IActionAuthorizer>("Authorization.Authorizer");
        authorizer.Grant("user-1", "ship-1", FireCommand.FireAction);
        new RegisterIoCDependencyTorpedo().Execute();
        new RegisterIoCDependencyFireCommand().Execute();

        var fire = Ioc.Resolve<SpaceBattle.lib.ICommand>(
            "Commands.Fire",
            "user-1",
            "ship-1",
            shooter.Object,
            new Vector(-5, 12),
            receiver.Object);

        fire.Execute();

        repository.Verify(r => r.Add(It.IsAny<string>(), It.IsAny<ITorpedo>()), Times.Once());
        moveOperation.Verify(m => m.Execute(), Times.Once());
    }

    static void RegisterFireTestInfrastructure(IGameObjectRepository repository, SpaceBattle.lib.ICommand moveOperation)
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Registry.Repository",
            (object[] _) => repository
        ).Execute();

        new RegisterIoCDependencyGameObjectRepository().Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.Move",
            (object[] _) => moveOperation
        ).Execute();

        new RegisterIoCDependencyActionsStart().Execute();
    }
}
