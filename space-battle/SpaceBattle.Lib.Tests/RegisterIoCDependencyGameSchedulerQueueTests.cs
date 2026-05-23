using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class RegisterIoCDependencyGameSchedulerQueueTests
{
    public RegisterIoCDependencyGameSchedulerQueueTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_RegistersGameSchedulerQueue_AndResolveTake()
    {
        new RegisterIoCDependencyGameSchedulerQueue().Execute();

        var cmd = new Mock<SpaceBattle.lib.ICommand>();
        var queue = Ioc.Resolve<IGameSchedulerQueue>("Game.Scheduler.Queue");
        queue.Receive(cmd.Object);

        Assert.Equal(1, queue.Count);
        queue.Take().Execute();
        cmd.Verify(c => c.Execute(), Times.Once());
        Assert.Equal(0, queue.Count);
    }
}
