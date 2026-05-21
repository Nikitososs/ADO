
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyGameObjectRepository : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Registry.Get",
            (object[] args) =>
            {
                var repo = Ioc.Resolve<IGameObjectRepository>("Game.Registry.Repository");
                return repo.Get((string)args[0]);
            }
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Registry.Create",
            (object[] args) =>
            {
                var repo = Ioc.Resolve<IGameObjectRepository>("Game.Registry.Repository");
                return new GameRegistryCreateCommand(repo, (string)args[0], args[1]);
            }
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Registry.Delete",
            (object[] args) =>
            {
                var repo = Ioc.Resolve<IGameObjectRepository>("Game.Registry.Repository");
                return new GameRegistryDeleteCommand(repo, (string)args[0]);
            }
        ).Execute();
    }
}
