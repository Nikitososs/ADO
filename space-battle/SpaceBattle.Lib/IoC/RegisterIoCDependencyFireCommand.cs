
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyFireCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.Fire",
            (object[] args) =>
            {
                var userId = (string)args[0];
                var shipObjectId = (string)args[1];
                var shooter = (IShootable)args[2];
                var initialVelocity = (Vector)args[3];
                var commandReceiver = (ICommandReceiver)args[4];

                var actionAuthorizer = Ioc.Resolve<IActionAuthorizer>("Authorization.Authorizer");
                var torpedoFactory = Ioc.Resolve<ITorpedoFactory>("Torpedo.Factory");

                return new FireCommand(
                    userId,
                    shipObjectId,
                    shooter,
                    initialVelocity,
                    actionAuthorizer,
                    torpedoFactory,
                    commandReceiver);
            }
        ).Execute();
    }
}
