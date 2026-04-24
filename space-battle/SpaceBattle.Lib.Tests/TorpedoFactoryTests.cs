using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class RegisterIoCDependencyTorpedoFactoryTest
{
    public RegisterIoCDependencyTorpedoFactoryTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_RegistersTorpedoFactory_AndCreatesTorpedoFromShip()
    {
        new RegisterIoCDependencyTorpedoFactory().Execute();
        var ship = new Mock<IShip>();
        ship.SetupGet(s => s.Position).Returns(new Vector([3, 7]));
        ship.SetupGet(s => s.Direction).Returns(new Vector([0, 2]));

        var factory = Ioc.Resolve<ITorpedoFactory>("Factories.TorpedoFactory");
        var torpedo = factory.Create(ship.Object);

        Assert.NotNull(factory);
        Assert.NotNull(torpedo);
        Assert.Equal(new Vector([3, 7]), torpedo.Position);
        Assert.Equal(new Vector([0, 2]), torpedo.Velocity);
    }
}
