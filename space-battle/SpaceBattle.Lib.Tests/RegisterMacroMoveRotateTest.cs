
using App;
using App.Scopes;
using Moq;
using SpaceBattle.lib;
using Xunit;

public class RegisterMacroMoveRotateTests
{
    public RegisterMacroMoveRotateTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_RegistersMacroMoveAndRotate_BothWork()
    {
        var moveCmd = new Mock<SpaceBattle.lib.ICommand>();
        var rotateCmd = new Mock<SpaceBattle.lib.ICommand>();
        var targetObject = new object();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Move", (object[] args) =>
            new List<string> { "Command.MoveDoDep" }).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Command.MoveDoDep", (object[] args) => moveCmd.Object).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Rotate", (object[] args) =>
            new List<string> { "Command.RotateDoDep" }).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Command.RotateDoDep", (object[] args) => rotateCmd.Object).Execute();

        new RegisterIoCDependencyMacroMoveRotate().Execute();

        var macroMove = Ioc.Resolve<SpaceBattle.lib.ICommand>("Macro.Move", targetObject);
        macroMove.Execute();

        var macroRotate = Ioc.Resolve<SpaceBattle.lib.ICommand>("Macro.Rotate", targetObject);
        macroRotate.Execute();

        moveCmd.Verify(m => m.Execute(), Times.Once());
        rotateCmd.Verify(m => m.Execute(), Times.Once());
    }
}
