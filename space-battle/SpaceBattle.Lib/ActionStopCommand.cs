
namespace SpaceBattle.lib;

public class ActionStopCommand : SpaceBattle.lib.ICommand
{
    private readonly object _target;

    public ActionStopCommand(params object[] args)
    {
        _target = args[0];
    }

    public void Execute()
    {

    }
}