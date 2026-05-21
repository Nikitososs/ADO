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
        new RegisterIoCDependencyGame().Execute();

        var cmd1 = new Mock<SpaceBattle.lib.ICommand>();
        var cmd2 = new Mock<SpaceBattle.lib.ICommand>();
        var queue = (GameSchedulerQueue)Ioc.Resolve<IQueue>("Game.Scheduler.Queue");
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
        new RegisterIoCDependencyGame().Execute();

        var failing = new Mock<SpaceBattle.lib.ICommand>();
        failing.Setup(c => c.Execute()).Throws<InvalidOperationException>();
        var ok = new Mock<SpaceBattle.lib.ICommand>();

        var queue = (GameSchedulerQueue)Ioc.Resolve<IQueue>("Game.Scheduler.Queue");
        queue.Receive(failing.Object);
        queue.Receive(ok.Object);

        var gameScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Game", gameScope).Execute();

        failing.Verify(c => c.Execute(), Times.Once());
        ok.Verify(c => c.Execute(), Times.Once());
    }

    [Fact]
    public void GameSchedulerQueue_ReceiveAndTake_WorksWithoutIoc()
    {
        var queue = new GameSchedulerQueue();
        var cmd = new Mock<SpaceBattle.lib.ICommand>();

        queue.Receive(cmd.Object);

        Assert.Equal(1, queue.Count);
        queue.Take().Execute();
        cmd.Verify(c => c.Execute(), Times.Once());
        Assert.Equal(0, queue.Count);
    }
}
