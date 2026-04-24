using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class GameTests
{
    [Fact]
    public void Step_ExecutesQueuedCommands_InOrder_ThenSimulationStep()
    {
        var order = 0;
        var seen = new List<int>();
        var cmd1 = new Mock<SpaceBattle.lib.ICommand>();
        var cmd2 = new Mock<SpaceBattle.lib.ICommand>();
        var simulationStep = new Mock<SpaceBattle.lib.ICommand>();
        cmd1.Setup(c => c.Execute()).Callback(() => seen.Add(++order));
        cmd2.Setup(c => c.Execute()).Callback(() => seen.Add(++order));
        simulationStep.Setup(c => c.Execute()).Callback(() => seen.Add(99));

        var game = new Game(simulationStep.Object);
        game.Receive(cmd1.Object);
        game.Receive(cmd2.Object);
        game.Step();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
        simulationStep.Verify(c => c.Execute(), Times.Once);
        Assert.Equal(new[] { 1, 2, 99 }, seen);
    }

    [Fact]
    public void Step_WithEmptyQueue_StillRunsSimulationStep()
    {
        var simulationStep = new Mock<SpaceBattle.lib.ICommand>();
        new Game(simulationStep.Object).Step();
        simulationStep.Verify(c => c.Execute(), Times.Once);
    }
}

public class RegisterIoCDependencyGameTests
{
    public RegisterIoCDependencyGameTests()
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
    public void E2E_Step_AfterFire_MovesNewTorpedo()
    {
        new RegisterIoCDependencyFireAuthorizer().Execute();
        new RegisterIoCDependencyFireCommand().Execute();
        new RegisterIoCDependencyMoveTorpedoesCommand().Execute();
        new RegisterIoCDependencySimulationStep().Execute();
        new RegisterIoCDependencyGame().Execute();
        var repository = Ioc.Resolve<IGameObjectRepository>("Repositories.GameObjects");
        var ship = new Mock<IShip>();
        ship.SetupGet(s => s.Position).Returns(new Vector([5, 5]));
        ship.SetupGet(s => s.Direction).Returns(new Vector([0, 3]));
        var fire = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Fire", ship.Object);
        var game = Ioc.Resolve<IGame>("Game");
        game.Receive(fire);
        game.Step();

        Assert.Single(repository.GetAll());
        var torpedo = repository.GetAll().OfType<ITorpedo>().Single();
        Assert.Equal(new Vector([5, 8]), torpedo.Position);
    }

    [Fact]
    public void RegisterIoCDependencySimulationStep_UsesSpec_AndExecutesMoveTorpedoes()
    {
        new RegisterIoCDependencyGameObjectRepository().Execute();
        new RegisterIoCDependencyMoveTorpedoesCommand().Execute();
        new RegisterIoCDependencySimulationStep().Execute();
        var spec = Ioc.Resolve<IEnumerable<string>>("Specs.Simulation");
        Assert.Equal(new[] { "Commands.MoveTorpedoes" }, spec);
        var repository = Ioc.Resolve<IGameObjectRepository>("Repositories.GameObjects");
        var torpedo = new Torpedo(new Vector([0, 0]), new Vector([2, 0]));
        repository.Add("t1", torpedo);
        var step = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.SimulationStep");
        step.Execute();
        Assert.Equal(new Vector([2, 0]), torpedo.Position);
    }
}
