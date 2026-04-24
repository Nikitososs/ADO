using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyFireCommand : ICommand
{
    public void Execute()
    {
        new RegisterIoCDependencyGameObjectRepository().Execute();
        new RegisterIoCDependencyTorpedoFactory().Execute();
        new RegisterIoCDependencyIdGenerator().Execute();
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Rules.Fire",
            (object[] args) =>
            {
                var idGenerator = Ioc.Resolve<IIdGenerator>("Generators.Id");
                return new IFireRule[]
                {
                    new CreateTorpedoFireRule(idGenerator),
                    new StoreTorpedoFireRule(),
                };
            }
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.Fire",
            (object[] args) =>
            {
                var ship = (IShip)args[0];
                var torpedoFactory = Ioc.Resolve<ITorpedoFactory>("Factories.TorpedoFactory");
                var gameObjectRepository = Ioc.Resolve<IGameObjectRepository>("Repositories.GameObjects");
                var fireRules = Ioc.Resolve<IEnumerable<IFireRule>>("Rules.Fire");

                return new FireCommand(torpedoFactory, gameObjectRepository, ship, fireRules);
            }
        ).Execute();
    }
}
