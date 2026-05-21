
namespace SpaceBattle.lib;

public class SetSchedulerStartDateCommand(SchedulerState state, int startDate) : ICommand
{
    public void Execute()
    {
        state.StartDate = startDate;
        state.CurrentDate = startDate;
    }
}
