
using App;

namespace SpaceBattle.lib;

public class BeginGameQuantumCommand(SchedulerState state) : ICommand
{
    public void Execute()
    {
        state.StartDate = Ioc.Resolve<int>("Game.Scheduler.CurrentDate.Get");
        state.CurrentDate = state.StartDate;

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Scheduler.CanContinue",
            (object[] _) => (object)CanContinueInProgress(state)
        ).Execute();
    }

    static bool CanContinueInProgress(SchedulerState state)
    {
        var queue = Ioc.Resolve<IQueue>("Game.Scheduler.Queue");
        return state.CurrentDate - state.StartDate < state.Quantum && queue.Count > 0;
    }
}
