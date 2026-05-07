
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyMoveCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.Move",
            (object[] args) =>
            {
                var obj = args[0];

                var movingObject = Ioc.Resolve<IMovingObject>("Adapters.IMovingObject", obj);
                return new MoveCommand(movingObject);
            }
        ).Execute();
    }
}
