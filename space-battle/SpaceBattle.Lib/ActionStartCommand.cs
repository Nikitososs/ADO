
namespace SpaceBattle.lib;

public class ActionStartCommand : SpaceBattle.lib.ICommand
{
    private readonly ICommand _longRunningOperation;
    private readonly ICommandReceiver _receiver;

    public ActionStartCommand(ICommand longRunningOperation, ICommandReceiver receiver)
    {
        _longRunningOperation = longRunningOperation;
        _receiver = receiver;
    }

    public void Execute()
    {
        new MacroCommand(
            [
                _longRunningOperation,
                new SendCommand(this, _receiver),
            ]
        ).Execute();
    }
}
