
namespace SpaceBattle.lib;

public class SchedulerState
{
    public int StartDate { get; set; }

    public int CurrentDate { get; set; }

    public int Quantum { get; set; } = 100;
}
