using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class MoveTorpedoesCommandTests
{
    public MoveTorpedoesCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Adapters.IMovingObject",
            (object[] args) => (IMovingObject)args[0]
        ).Execute();
        new RegisterIoCDependencyMoveCommand().Execute();
    }

    [Fact]
    public void Execute_MovesEachTorpedoByVelocity()
    {
        IGameObjectRepository repository = new GameObjectRepository();
        var t1 = new Torpedo(new Vector([0, 0]), new Vector([1, 2]));
        var t2 = new Torpedo(new Vector([10, 10]), new Vector([-1, 0]));
        repository.Add("t1", t1);
        repository.Add("t2", t2);
        repository.Add("other", new Mock<IGameObject>().Object);

        new MoveTorpedoesCommand(repository).Execute();

        Assert.Equal(new Vector([1, 2]), t1.Position);
        Assert.Equal(new Vector([9, 10]), t2.Position);
    }
}

public class RegisterIoCDependencyMoveTorpedoesCommandTests
{
    public RegisterIoCDependencyMoveTorpedoesCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Adapters.IMovingObject",
            (object[] args) => (IMovingObject)args[0]
        ).Execute();
        new RegisterIoCDependencyMoveCommand().Execute();
    }

    [Fact]
    public void Execute_RegistersMoveTorpedoes_AndMovesTorpedoesInRepository()
    {
        new RegisterIoCDependencyGameObjectRepository().Execute();
        new RegisterIoCDependencyMoveTorpedoesCommand().Execute();
        var repository = Ioc.Resolve<IGameObjectRepository>("Repositories.GameObjects");
        var torpedo = new Torpedo(new Vector([0, 0]), new Vector([3, 0]));
        repository.Add("p1", torpedo);

        var cmd = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.MoveTorpedoes");
        cmd.Execute();

        Assert.Equal(new Vector([3, 0]), torpedo.Position);
    }
}
