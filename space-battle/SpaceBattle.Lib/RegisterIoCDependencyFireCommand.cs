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
                var accessToken = args.Length > 1 ? (string)args[1] : null;
                var torpedoFactory = Ioc.Resolve<ITorpedoFactory>("Factories.TorpedoFactory");
                var gameObjectRepository = Ioc.Resolve<IGameObjectRepository>("Repositories.GameObjects");
                var fireRules = Ioc.Resolve<IEnumerable<IFireRule>>("Rules.Fire");
                var authorizer = Ioc.Resolve<IFireAuthorizer>("Authorizers.Fire");
                var fire = new FireCommand(torpedoFactory, gameObjectRepository, ship, fireRules);
                return new AuthorizedFireCommand(fire, authorizer, ship, accessToken);
            }
        ).Execute();
    }
}
