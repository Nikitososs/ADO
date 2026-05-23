
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyGame : ICommand
{
    public void Execute()
    {
        var state = new SchedulerState();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.Take",
            (object[] _) => Ioc.Resolve<IGameSchedulerQueue>("Game.Scheduler.Queue").Take()
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.State",
            (object[] _) => state
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.CurrentDate.Get",
            (object[] _) => (object)state.CurrentDate
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.Quantum",
            (object[] _) => (object)state.Quantum
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.CanContinue",
            (object[] _) => (object)(Ioc.Resolve<IGameSchedulerQueue>("Game.Scheduler.Queue").Count > 0)
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.BeginQuantum",
            (object[] _) => new BeginGameQuantumCommand(state)
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.CurrentDate.Advance",
            (object[] _) => new AdvanceSchedulerDateCommand(state)
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.StartDate.Set",
            (object[] args) => new SetSchedulerStartDateCommand(state, (int)args[0])
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.Game",
            (object[] args) => new GameCommand(args[0])
        ).Execute();
    }
}
