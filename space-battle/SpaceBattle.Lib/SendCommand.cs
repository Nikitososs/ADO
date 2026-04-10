
namespace SpaceBattle.lib;

public class SendCommand(ICommand sendingCommand, ICommandReceiver receiver) : ICommand
{
    public void Execute()
    {
        receiver.Receive(sendingCommand);
    }
}
