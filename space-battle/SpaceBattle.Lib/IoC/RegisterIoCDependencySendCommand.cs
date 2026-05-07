
using App;
namespace SpaceBattle.lib;

public class RegisterIoCDependencySendCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.SendCommand",
            (object[] args) => new SendCommand((ICommand)args[0], (ICommandReceiver)args[1])
        ).Execute();
    }
}
