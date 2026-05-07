
using App;
namespace SpaceBattle.lib;

public class RegisterDependencyCommandInjectableCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.CommandInjectable",
            (object[] args) => new CommandInjectableCommand((ICommand)args[0])
        ).Execute();
    }
}
