using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class GameObjectRepositoryTests
{
    [Fact]
    public void Repository_AddTryGetRemove_WorksCorrectly()
    {
        IGameObjectRepository repository = new GameObjectRepository();
        var gameObject = new Mock<IGameObject>();
        var id = "ship-1";

        repository.Add(id, gameObject.Object);
        var canRead = repository.TryGet(id, out var fromRepository);
        var wasRemoved = repository.Remove(id);
        var existsAfterRemove = repository.TryGet(id, out _);

        Assert.True(canRead);
        Assert.Same(gameObject.Object, fromRepository);
        Assert.True(wasRemoved);
        Assert.False(existsAfterRemove);
    }

    [Fact]
    public void Repository_GetAll_ReturnsAllStoredObjects()
    {
        IGameObjectRepository repository = new GameObjectRepository();
        var first = new Mock<IGameObject>();
        var second = new Mock<IGameObject>();
        repository.Add("a", first.Object);
        repository.Add("b", second.Object);

        var all = repository.GetAll();

        Assert.Equal(2, all.Count);
        Assert.Contains(first.Object, all);
        Assert.Contains(second.Object, all);
    }
}

public class RegisterIoCDependencyGameObjectRepositoryTest
{
    public RegisterIoCDependencyGameObjectRepositoryTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_RegistersGameObjectsRepository_AndResolves()
    {
        new RegisterIoCDependencyGameObjectRepository().Execute();

        var repository = Ioc.Resolve<IGameObjectRepository>("Repositories.GameObjects");

        Assert.NotNull(repository);
        Assert.IsType<GameObjectRepository>(repository);
    }
}
