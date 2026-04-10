
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyMacroMoveRotate : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Macro.Move",
            (object[] args) => new CreateMacroCommandStrategy("Specs.Move").Resolve(args)
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Macro.Rotate",
            (object[] args) => new CreateMacroCommandStrategy("Specs.Rotate").Resolve(args)
        ).Execute();
    }
}
