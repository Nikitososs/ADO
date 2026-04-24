using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class FireAuthorizationTests
{
    public FireAuthorizationTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void FixedToken_Allows_WhenTokenMatches()
    {
        new RegisterIoCDependencyFixedTokenFireAuthorizer("secret").Execute();
        new RegisterIoCDependencyFireCommand().Execute();
        var ship = new Mock<IShip>();
        var fire = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Fire", ship.Object, "secret");
        var repository = Ioc.Resolve<IGameObjectRepository>("Repositories.GameObjects");
        var before = repository.GetAll().Count;
        fire.Execute();
        Assert.Equal(before + 1, repository.GetAll().Count);
    }

    [Fact]
    public void FixedToken_Denies_WhenTokenMissing()
    {
        new RegisterIoCDependencyFireCommand().Execute();
        new RegisterIoCDependencyFixedTokenFireAuthorizer("secret").Execute();
        var ship = new Mock<IShip>();
        var fire = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Fire", ship.Object);
        var repository = Ioc.Resolve<IGameObjectRepository>("Repositories.GameObjects");
        fire.Execute();
        Assert.Empty(repository.GetAll());
    }

    [Fact]
    public void FixedToken_Denies_WhenTokenWrong()
    {
        new RegisterIoCDependencyFixedTokenFireAuthorizer("secret").Execute();
        new RegisterIoCDependencyFireCommand().Execute();
        var ship = new Mock<IShip>();
        var fire = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Fire", ship.Object, "wrong");
        var repository = Ioc.Resolve<IGameObjectRepository>("Repositories.GameObjects");
        fire.Execute();
        Assert.Empty(repository.GetAll());
    }
}
