
namespace SpaceBattle.lib;

public class AdvanceSchedulerDateCommand(SchedulerState state) : ICommand
{
    public void Execute()
    {
        state.CurrentDate++;
    }
}
