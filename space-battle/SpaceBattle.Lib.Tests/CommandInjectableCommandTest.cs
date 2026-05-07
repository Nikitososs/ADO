
using Xunit;
using Moq;
using App;
using SpaceBattle.lib;
using App.Scopes;
public class CommandInjectableCommandTests
{
    [Fact]
    public void CommandExecutingAfterInjection()
    {
        var cmd1 = new Mock<SpaceBattle.lib.ICommand>();
        var cmd2 = new Mock<SpaceBattle.lib.ICommand>();
        var injectable = new CommandInjectableCommand(cmd1.Object);

        injectable.Execute();
        injectable.Inject(cmd2.Object);
        injectable.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void CommandInjectableCommandThrowsExceptionIfCommandWasNotInjected()
    {
        var injectable = new CommandInjectableCommand(null);

        Assert.Throws<InvalidOperationException>(() => injectable.Execute());
    }

    [Fact]
    public void CommandInjectableCommandThrowsExceptionIfInjectedNull()
    {
        var cmd = new Mock<SpaceBattle.lib.ICommand>();
        var injectable = new CommandInjectableCommand(cmd.Object);

        Assert.Throws<ArgumentNullException>(() => injectable.Inject(null));
    }
}

public class RegisterDependencyCommandInjectableCommandTest
{
    public RegisterDependencyCommandInjectableCommandTest()
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

        new RegisterDependencyCommandInjectableCommand().Execute();

        var injectable = Ioc.Resolve<ICommandInjectable>("Commands.CommandInjectable", cmd1.Object);
        ((SpaceBattle.lib.ICommand)injectable).Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);

        injectable.Inject(cmd2.Object);

        ((SpaceBattle.lib.ICommand)injectable).Execute();

        cmd1.Verify(c => c.Execute(), Times.Once);
        cmd2.Verify(c => c.Execute(), Times.Once);
    }

    [Fact]
    public void ShouldResolveAsMultipleTypesWithoutException()
    {
        new RegisterDependencyCommandInjectableCommand().Execute();
        var mockCmd = new Mock<SpaceBattle.lib.ICommand>().Object;


        var icmd = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.CommandInjectable", mockCmd);
        Assert.NotNull(icmd);

        var injc = Ioc.Resolve<ICommandInjectable>("Commands.CommandInjectable", mockCmd);
        Assert.NotNull(injc);

        var cmd = Ioc.Resolve<CommandInjectableCommand>("Commands.CommandInjectable", mockCmd);
        Assert.NotNull(cmd);
    }
}
