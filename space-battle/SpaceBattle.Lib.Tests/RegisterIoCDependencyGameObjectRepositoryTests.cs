using App;
using App.Scopes;
using SpaceBattle.lib;

public class RegisterIoCDependencyGameObjectRepositoryTests
{
    public RegisterIoCDependencyGameObjectRepositoryTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_RegistersRepository_AndResolveGetCreateDelete()
    {
        new RegisterIoCDependencyGameObjectRepository().Execute();

        var repository = Ioc.Resolve<IGameObjectRepository>("Game.Registry.Repository");
        Assert.IsType<GameObjectRepository>(repository);

        var payload = new object();
        const string objectId = "torpedo-1";

        Ioc.Resolve<SpaceBattle.lib.ICommand>("Game.Registry.Create", objectId, payload).Execute();

        var resolved = Ioc.Resolve<object>("Game.Registry.Get", objectId);
        Assert.Same(payload, resolved);

        Ioc.Resolve<SpaceBattle.lib.ICommand>("Game.Registry.Delete", objectId).Execute();

        Assert.Throws<KeyNotFoundException>(() => Ioc.Resolve<object>("Game.Registry.Get", objectId));
    }

    [Fact]
    public void GameRegistryCreateCommand_StoresObjectInRepository()
    {
        new RegisterIoCDependencyGameObjectRepository().Execute();

        var payload = new object();
        const string objectId = "ship-1";

        Ioc.Resolve<SpaceBattle.lib.ICommand>("Game.Registry.Create", objectId, payload).Execute();

        Assert.Same(payload, Ioc.Resolve<object>("Game.Registry.Get", objectId));
    }
}
