using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class RegisterIoCDependencyGameObjectRepositoryTests
{
    public RegisterIoCDependencyGameObjectRepositoryTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    static void RegisterRepository(IGameObjectRepository repository)
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Registry.Repository",
            (object[] _) => repository
        ).Execute();
    }

    [Fact]
    public void Execute_RegistersGameRegistry_AndResolveGetCreateDelete()
    {
        var store = new Dictionary<string, object>();
        var repo = new Mock<IGameObjectRepository>();
        repo.Setup(r => r.Add(It.IsAny<string>(), It.IsAny<object>()))
            .Callback<string, object>((id, obj) => store[id] = obj);
        repo.Setup(r => r.Get(It.IsAny<string>()))
            .Returns<string>(id => store.TryGetValue(id, out var obj)
                ? obj
                : throw new KeyNotFoundException());
        repo.Setup(r => r.Remove(It.IsAny<string>()))
            .Callback<string>(id => store.Remove(id));

        RegisterRepository(repo.Object);
        new RegisterIoCDependencyGameObjectRepository().Execute();

        var payload = new object();
        const string objectId = "torpedo-1";

        Ioc.Resolve<SpaceBattle.lib.ICommand>("Game.Registry.Create", objectId, payload).Execute();

        var resolved = Ioc.Resolve<object>("Game.Registry.Get", objectId);
        Assert.Same(payload, resolved);

        Ioc.Resolve<SpaceBattle.lib.ICommand>("Game.Registry.Delete", objectId).Execute();

        Assert.Throws<KeyNotFoundException>(() => Ioc.Resolve<object>("Game.Registry.Get", objectId));
        repo.Verify(r => r.Add(objectId, payload), Times.Once());
        repo.Verify(r => r.Remove(objectId), Times.Once());
    }

    [Fact]
    public void GameRegistryCreateCommand_CallsRepositoryAdd()
    {
        var repo = new Mock<IGameObjectRepository>();
        RegisterRepository(repo.Object);
        new RegisterIoCDependencyGameObjectRepository().Execute();

        var payload = new object();
        Ioc.Resolve<SpaceBattle.lib.ICommand>("Game.Registry.Create", "ship-1", payload).Execute();

        repo.Verify(r => r.Add("ship-1", payload), Times.Once());
    }
}
