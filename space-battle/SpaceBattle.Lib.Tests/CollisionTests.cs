using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class CollisionTests
{
    private static ICollisionChecker NoCollision => Mock.Of<ICollisionChecker>();

    private static string TorpedoProfilePath =>
        Path.Combine(AppContext.BaseDirectory, "CollisionData", "torpedo.txt");

    private static string ShipProfilePath =>
        Path.Combine(AppContext.BaseDirectory, "CollisionData", "ship.txt");

    private static PrecomputedCollisionChecker LoadTorpedoProfile() =>
        PrecomputedCollisionChecker.Load(TorpedoProfilePath);

    private static PrecomputedCollisionChecker LoadShipProfile() =>
        PrecomputedCollisionChecker.Load(ShipProfilePath);

    [Fact]
    public void PrecomputedCollisionChecker_LoadsPositiveEntries()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "CollisionData", "point.txt");
        var checker = PrecomputedCollisionChecker.Load(path);

        Assert.True(checker.Collides(1, 0, -1, 0));
        Assert.True(checker.Collides(0, 1, 0, -1));
        Assert.False(checker.Collides(2, 0, -1, 0));
        Assert.False(checker.Collides(1, 0, 0, 0));
    }

    [Fact]
    public void PrecomputedCollisionChecker_RejectsInvalidFile()
    {
        var path = Path.GetTempFileName();
        File.WriteAllText(path, "1 2 3");

        try
        {
            Assert.Throws<InvalidDataException>(() => PrecomputedCollisionChecker.Load(path));
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void TorpedoProfile_LoadsGeneratedFile()
    {
        var profile = LoadTorpedoProfile();

        Assert.True(File.ReadAllLines(TorpedoProfilePath).Length >= 500);
        Assert.True(profile.Collides(-5, -5, 5, 5));
        Assert.False(profile.Collides(99, 99, 0, 0));
    }

    [Fact]
    public void ShipProfile_LoadsGeneratedFile()
    {
        var profile = LoadShipProfile();

        Assert.True(File.ReadAllLines(ShipProfilePath).Length >= 2800);
        Assert.True(profile.Collides(-6, -6, 5, 5));
        Assert.False(profile.Collides(99, 99, 0, 0));
    }

    [Fact]
    public void WillCollide_WithRealTorpedoProfile_UsesPointVsTorpedoEntry()
    {
        var ship = new Ship("ship", new Vector(0, 0), new Vector(0, 0), new Angle(0), LoadShipProfile());
        var torpedo = new Torpedo("torpedo", new Vector(-5, -5), new Vector(5, 5), LoadTorpedoProfile());

        Assert.True(PrecomputedCollisionChecker.WillCollide(ship, torpedo));
    }

    [Fact]
    public void WillCollide_WithRealShipProfile_UsesPointVsShipEntry()
    {
        var torpedo = new Torpedo("torpedo", new Vector(0, 0), new Vector(0, 0), LoadTorpedoProfile());
        var ship = new Ship("ship", new Vector(-6, -6), new Vector(5, 5), new Angle(0), LoadShipProfile());

        Assert.True(PrecomputedCollisionChecker.WillCollide(torpedo, ship));
    }

    [Fact]
    public void CheckCollisions_WithRealTorpedoProfile_ThrowsForRecordedCollision()
    {
        var repository = new DictionaryGameObjectRepository();
        var spatialIndex = new UniformGridSpatialIndex();
        var ship = new Ship("ship", new Vector(0, 0), new Vector(0, 0), new Angle(0), LoadShipProfile());
        var torpedo = new Torpedo("torpedo", new Vector(-1, -1), new Vector(1, 1), LoadTorpedoProfile());

        repository.Add("ship", ship);
        repository.Add("torpedo", torpedo);
        spatialIndex.Insert("ship", ship.Position);
        spatialIndex.Insert("torpedo", torpedo.Position);

        Assert.Throws<InvalidOperationException>(() =>
            new CheckCollisionsCommand(ship, spatialIndex, repository).Execute());
    }

    [Fact]
    public void TorpedoFactory_WithRealProfilePath_CreatesTorpedoWithLoadedChecker()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Collision.TorpedoProfile.Path",
            (object[] _) => TorpedoProfilePath
        ).Execute();
        new RegisterIoCDependencyCollision().Execute();
        new RegisterIoCDependencyTorpedo().Execute();

        var shooter = new Mock<IShootable>();
        shooter.SetupGet(s => s.Position).Returns(new Vector(0, 0));
        shooter.SetupGet(s => s.Facing).Returns(new Angle(0));

        var factory = Ioc.Resolve<ITorpedoFactory>("Torpedo.Factory");
        var torpedo = (Torpedo)factory.Create(shooter.Object, new Vector(5, 5));

        Assert.True(torpedo.CollisionChecker.Collides(-5, -5, 5, 5));
    }

    [Fact]
    public void WillCollide_UsesTargetProfileRelativeToReferencePoint()
    {
        var profile = new Mock<ICollisionChecker>();
        profile.Setup(c => c.Collides(2, 0, -1, 0)).Returns(true);

        var ship = new Ship("ship", new Vector(0, 0), new Vector(0, 0), new Angle(0), NoCollision);
        var torpedo = new Torpedo("torpedo", new Vector(2, 0), new Vector(-1, 0), profile.Object);

        Assert.True(PrecomputedCollisionChecker.WillCollide(ship, torpedo));
        profile.Verify(c => c.Collides(2, 0, -1, 0), Times.Once);
    }

    [Fact]
    public void UniformGridSpatialIndex_ReturnsNearbyObjects()
    {
        var index = new UniformGridSpatialIndex();
        index.Insert("a", new Vector(0, 0));
        index.Insert("b", new Vector(5, 0));

        var nearby = index.QueryNearby(new Vector(0, 0)).ToList();

        Assert.Contains("a", nearby);
        Assert.DoesNotContain("b", nearby);
    }

    [Fact]
    public void UniformGridSpatialIndex_UpdatesObjectCell()
    {
        var index = new UniformGridSpatialIndex();
        index.Insert("a", new Vector(0, 0));
        index.Update("a", new Vector(0, 0), new Vector(3, 0));

        Assert.DoesNotContain("a", index.QueryNearby(new Vector(0, 0)));
        Assert.Contains("a", index.QueryNearby(new Vector(3, 0)));
    }

    [Fact]
    public void MacroMove_StopsOnCollision()
    {
        var repository = new Mock<IGameObjectRepository>();
        var spatialIndex = new UniformGridSpatialIndex();
        var profile = new Mock<ICollisionChecker>();
        profile.Setup(c => c.Collides(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

        var ship = new Ship("ship", new Vector(0, 0), new Vector(1, 0), new Angle(0), NoCollision);
        var torpedo = new Torpedo("torpedo", new Vector(1, 0), new Vector(0, 0), profile.Object);

        repository.Setup(r => r.Get("torpedo")).Returns(torpedo);
        spatialIndex.Insert("ship", ship.Position);
        spatialIndex.Insert("torpedo", torpedo.Position);

        Assert.Throws<InvalidOperationException>(() =>
            new CheckCollisionsCommand(ship, spatialIndex, repository.Object).Execute());

        Assert.Equal(new Vector(0, 0), ship.Position);
    }

    [Fact]
    public void MacroMove_MovesWhenNoCollision()
    {
        var repository = new Mock<IGameObjectRepository>();
        var spatialIndex = new UniformGridSpatialIndex();
        var ship = new Ship("ship", new Vector(0, 0), new Vector(1, 0), new Angle(0), NoCollision);

        spatialIndex.Insert("ship", ship.Position);

        new CheckCollisionsCommand(ship, spatialIndex, repository.Object).Execute();
        new MoveCommand(ship).Execute();
        new UpdateSpatialIndexCommand(ship, spatialIndex).Execute();

        Assert.Equal(new Vector(1, 0), ship.Position);
    }

    [Fact]
    public void GameRegistryCreateCommand_RegistersCollidableInSpatialIndex()
    {
        var repository = new Mock<IGameObjectRepository>();
        var spatialIndex = new UniformGridSpatialIndex();
        var ship = new Ship("ship", new Vector(2, 3), new Vector(0, 0), new Angle(0), NoCollision);

        new GameRegistryCreateCommand(repository.Object, "ship", ship, spatialIndex).Execute();

        repository.Verify(r => r.Add("ship", ship), Times.Once);
        Assert.Contains("ship", spatialIndex.QueryNearby(new Vector(2, 3)));
    }

    [Fact]
    public void Vector_SubtractsCoordinates()
    {
        var result = new Vector(5, 7) - new Vector(2, 3);

        Assert.Equal(new Vector(3, 4), result);
    }

    [Fact]
    public void RegisterIoCDependencyCollision_ResolvesMacroMove()
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
        new RegisterIoCDependencyMoveCommand().Execute();
        new RegisterIoCDependencyMacroMoveRotate().Execute();

        var repository = new DictionaryGameObjectRepository();
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game.Registry.Repository",
            (object[] _) => repository
        ).Execute();
        new RegisterIoCDependencyGameObjectRepository().Execute();

        var ship = new Ship("ship", new Vector(0, 0), new Vector(0, 0), new Angle(0), NoCollision);
        Ioc.Resolve<SpaceBattle.lib.ICommand>("Game.Registry.Create", "ship", ship).Execute();
        Ioc.Resolve<SpaceBattle.lib.ICommand>("Macro.Move", ship).Execute();

        Assert.Equal(new Vector(0, 0), ship.Position);
    }
}

internal sealed class DictionaryGameObjectRepository : IGameObjectRepository
{
    private readonly Dictionary<string, object> _objects = new(StringComparer.Ordinal);

    public void Add(string objectId, object gameObject) => _objects[objectId] = gameObject;

    public object Get(string objectId) => _objects[objectId];

    public void Remove(string objectId) => _objects.Remove(objectId);
}
