using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class GameTests
{
    public GameTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void GameCommand_ExecutesAllQueuedCommands()
    {
        var queue = new TestGameSchedulerQueue();
        RegisterSchedulerQueue(queue);
        new RegisterIoCDependencyGame().Execute();

        var cmd1 = new Mock<SpaceBattle.lib.ICommand>();
        var cmd2 = new Mock<SpaceBattle.lib.ICommand>();
        queue.Receive(cmd1.Object);
        queue.Receive(cmd2.Object);

        var gameScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Game", gameScope).Execute();

        cmd1.Verify(c => c.Execute(), Times.Once());
        cmd2.Verify(c => c.Execute(), Times.Once());
    }

    [Fact]
    public void GameCommand_WhenCommandThrows_InvokesExceptionHandler()
    {
        var queue = new TestGameSchedulerQueue();
        RegisterSchedulerQueue(queue);
        new RegisterIoCDependencyGame().Execute();

        var failing = new Mock<SpaceBattle.lib.ICommand>();
        failing.Setup(c => c.Execute()).Throws<InvalidOperationException>();
        var ok = new Mock<SpaceBattle.lib.ICommand>();

        queue.Receive(failing.Object);
        queue.Receive(ok.Object);

        var gameScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Game", gameScope).Execute();

        failing.Verify(c => c.Execute(), Times.Once());
        ok.Verify(c => c.Execute(), Times.Once());
    }

    static void RegisterSchedulerQueue(IGameSchedulerQueue queue)
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.Queue",
            (object[] _) => queue
        ).Execute();
    }
}

internal class TestGameSchedulerQueue : IGameSchedulerQueue
{
    private readonly Queue<SpaceBattle.lib.ICommand> _commands = new();

    public int Count => _commands.Count;

    public void Receive(SpaceBattle.lib.ICommand cmd)
    {
        _commands.Enqueue(cmd);
    }

    public SpaceBattle.lib.ICommand Take()
    {
        if (_commands.Count == 0)
            throw new InvalidOperationException("Scheduler queue is empty.");

        return _commands.Dequeue();
    }
}
