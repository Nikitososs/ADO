
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyGameSchedulerQueue : ICommand
{
    public void Execute()
    {
        var queue = new GameSchedulerQueue();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.Queue",
            (object[] _) => queue
        ).Execute();
    }
}
