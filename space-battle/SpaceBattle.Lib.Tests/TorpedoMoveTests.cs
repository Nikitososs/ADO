using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;

public class TorpedoMoveTests
{
    [Fact]
    public void MoveCommand_MovesTorpedoByVelocity()
    {
        var torpedo = new Torpedo("t1", new Vector(0, 0), new Vector(-5, 12));
        var move = new MoveCommand(torpedo);

        move.Execute();

        Assert.Equal(new Vector(-5, 12), torpedo.Position);
    }

    [Fact]
    public void MoveCommand_ResolvesTorpedoThroughMovingObjectAdapter()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        new RegisterIoCDependencyTorpedo().Execute();
        new RegisterIoCDependencyMoveCommand().Execute();

        var torpedo = new Torpedo("t1", new Vector(3, 4), new Vector(1, 2));
        var move = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.Move", torpedo);

        move.Execute();

        Assert.Equal(new Vector(4, 6), torpedo.Position);
    }
}
