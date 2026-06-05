using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class TorpedoFactoryTests
{
    [Fact]
    public void Create_ReturnsTorpedoAtShooterPositionWithGivenVelocity()
    {
        var shooter = new Mock<IShootable>();
        shooter.SetupGet(s => s.Position).Returns(new Vector(10, 20));
        shooter.SetupGet(s => s.Facing).Returns(new Angle(2));

        var idGenerator = new Mock<IIdGenerator>();
        idGenerator.Setup(g => g.NewId()).Returns("torpedo-42");

        var factory = new TorpedoFactory(idGenerator.Object, Mock.Of<ICollisionChecker>());
        var initialVelocity = new Vector(-5, 12);

        var torpedo = factory.Create(shooter.Object, initialVelocity);

        Assert.Equal("torpedo-42", torpedo.Id);
        Assert.Equal(new Vector(10, 20), ((Torpedo)torpedo).Position);
        Assert.Equal(initialVelocity, ((Torpedo)torpedo).Velocity);
        idGenerator.Verify(g => g.NewId(), Times.Once);
    }

    [Fact]
    public void Create_DoesNotSharePositionInstanceWithShooter()
    {
        var position = new Vector(1, 2);
        var shooter = new Mock<IShootable>();
        shooter.SetupGet(s => s.Position).Returns(position);

        var factory = new TorpedoFactory(new GuidIdGenerator(), Mock.Of<ICollisionChecker>());
        var torpedo = (Torpedo)factory.Create(shooter.Object, new Vector(1, 0));

        torpedo.Position = new Vector(9, 9);

        Assert.Equal(new Vector(1, 2), position);
    }

    [Fact]
    public void RegisterIoCDependencyTorpedo_ResolvesFactoryAndCreatesTorpedo()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        var path = Path.Combine(AppContext.BaseDirectory, "CollisionData", "point.txt");
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Collision.TorpedoProfile.Path",
            (object[] _) => path
        ).Execute();
        new RegisterIoCDependencyCollision().Execute();
        new RegisterIoCDependencyTorpedo().Execute();

        var shooter = new Mock<IShootable>();
        shooter.SetupGet(s => s.Position).Returns(new Vector(0, 0));
        shooter.SetupGet(s => s.Facing).Returns(new Angle(0));

        var factory = Ioc.Resolve<ITorpedoFactory>("Torpedo.Factory");
        var torpedo = factory.Create(shooter.Object, new Vector(-5, 12));

        Assert.IsType<Torpedo>(torpedo);
        Assert.Equal(new Vector(-5, 12), ((Torpedo)torpedo).Velocity);
        Assert.False(string.IsNullOrWhiteSpace(torpedo.Id));
    }
}
