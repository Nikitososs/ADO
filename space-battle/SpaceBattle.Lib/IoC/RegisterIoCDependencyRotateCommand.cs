
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyRotateCommand : lib.ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.Rotate",
            (object[] args) =>
            {
                var obj = args[0];

                var rotatable = Ioc.Resolve<IRotatableObject>("Adapters.IRotatingObject", obj);

                return new RotateCommand(rotatable);
            }
        ).Execute();
    }
}
