
using Xunit;
using Moq;
using SpaceBattle.lib;
using App;
using App.Scopes;

public class MacroCommandTest
{
    [Fact]
    public void MacroCommand_RunsAll()
    {
        var cmd1 = new Mock<SpaceBattle.lib.ICommand>();
        var cmd2 = new Mock<SpaceBattle.lib.ICommand>();
        var cmd3 = new Mock<SpaceBattle.lib.ICommand>();

        var macro = new MacroCommand([cmd1.Object, cmd2.Object, cmd3.Object]);

        macro.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once());
        cmd2.Verify(c => c.Execute(), Times.Once());
        cmd3.Verify(c => c.Execute(), Times.Once());
    }

    [Fact]
    public void MacroCommand_ThrowsException()
    {
        var cmd1 = new Mock<SpaceBattle.lib.ICommand>();
        var cmd2 = new Mock<SpaceBattle.lib.ICommand>();
        cmd2.Setup(cmd => cmd.Execute()).Throws<Exception>();
        var cmd3 = new Mock<SpaceBattle.lib.ICommand>();

        var macro = new MacroCommand([cmd1.Object, cmd2.Object, cmd3.Object]);

        Assert.Throws<Exception>(() => macro.Execute());

        cmd1.Verify(c => c.Execute(), Times.Once());
        cmd2.Verify(c => c.Execute(), Times.Once());
        cmd3.Verify(c => c.Execute(), Times.Never());
    }
}

public class RegisterIoCDependencyMacroCommandTest
{
    public RegisterIoCDependencyMacroCommandTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void General()
    {
        var cmd1 = new Mock<SpaceBattle.lib.ICommand>();
        var cmd2 = new Mock<SpaceBattle.lib.ICommand>();
        var cmd3 = new Mock<SpaceBattle.lib.ICommand>();

        new RegisterIoCDependencyMacroCommand().Execute();

        var macro = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.MacroCommand", new SpaceBattle.lib.ICommand[] { cmd1.Object, cmd2.Object, cmd3.Object });
        macro.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once());
        cmd2.Verify(c => c.Execute(), Times.Once());
        cmd3.Verify(c => c.Execute(), Times.Once());
    }
}
