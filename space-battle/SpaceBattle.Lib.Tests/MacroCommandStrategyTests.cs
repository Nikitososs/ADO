
using Xunit;
using Moq;
using SpaceBattle.lib;
using App.Scopes;
using App;

public class CreateMacroCommandStrategyTests
{
    public CreateMacroCommandStrategyTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Resolve_ValidDependencies_ReturnsMacroCommandAndExecutesAll()
    {
        var cmd1 = new Mock<SpaceBattle.lib.ICommand>();
        var cmd2 = new Mock<SpaceBattle.lib.ICommand>();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Test", (object[] args) => 
            new List<string> { "Command.Move", "Command.Rotate" }
        ).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Command.Move", (object[] args) => cmd1.Object).Execute();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Command.Rotate", (object[] args) => cmd2.Object).Execute();

        var strategy = new CreateMacroCommandStrategy("Specs.Test");

        var macro = strategy.Resolve(new object());
        macro.Execute();

        cmd1.Verify(m => m.Execute(), Times.Once());
        cmd2.Verify(m => m.Execute(), Times.Once());
    }

    [Fact]
    public void Resolve_MissingCommandDependency_ThrowsException()
    {
        Ioc.Resolve<App.ICommand>("IoC.Register", "Specs.Invalid", (object[] args) => 
            new List<string> { "Wrong.Cmd" }
        ).Execute();

        var strategy = new CreateMacroCommandStrategy("Specs.Invalid");
        Assert.ThrowsAny<Exception>(() => strategy.Resolve(new object()));
    }
}
