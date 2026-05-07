
using App;
namespace SpaceBattle.lib;

public class RegisterIoCDependencyMacroCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.MacroCommand",
            (object[] args) => new MacroCommand((ICommand[])args)
        ).Execute();
    }
}
