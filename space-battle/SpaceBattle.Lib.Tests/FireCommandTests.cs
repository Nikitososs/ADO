using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class FireCommandTests
{
    [Fact]
    public void Execute_CreatesTorpedo_AndStoresItInRepository()
    {
        var torpedoFactory = new Mock<ITorpedoFactory>();
        var repository = new Mock<IGameObjectRepository>();
        var ship = new Mock<IShip>();
        var torpedo = new Mock<ITorpedo>();
        var idGenerator = new Mock<IIdGenerator>();
        var createRule = new CreateTorpedoFireRule(idGenerator.Object);
        var storeRule = new StoreTorpedoFireRule();

        torpedoFactory.Setup(f => f.Create(ship.Object)).Returns(torpedo.Object);
        idGenerator.Setup(g => g.Generate()).Returns("projectile-id");

        var command = new FireCommand(
            torpedoFactory.Object,
            repository.Object,
            ship.Object,
            [createRule, storeRule]
        );
        command.Execute();

        torpedoFactory.Verify(f => f.Create(ship.Object), Times.Once);
        repository.Verify(
            r => r.Add(It.IsAny<string>(), It.Is<IGameObject>(g => ReferenceEquals(g, torpedo.Object))),
            Times.Once
        );
    }
}

public class RegisterIoCDependencyFireCommandTests
{
    public RegisterIoCDependencyFireCommandTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_RegistersFireCommand_AndResolveWorks()
    {
        var ship = new Mock<IShip>();
        new RegisterIoCDependencyFireCommand().Execute();
        var fire = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Fire", ship.Object);

        Assert.NotNull(fire);
        Assert.IsType<FireCommand>(fire);
        fire.Execute();
    }
}
