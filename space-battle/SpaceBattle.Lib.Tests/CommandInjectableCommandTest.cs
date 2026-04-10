
using Xunit;
using Moq;
using SpaceBattle.lib;
public class CommandInjectableCommandTests
{
    [Fact]
    public void CommandExecutingAfterInjection()
    {
        var cmd1 = new Mock<ICommand>();
        var cmd2 = new Mock<ICommand>();
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
        var cmd = new Mock<ICommand>();
        var injectable = new CommandInjectableCommand(cmd.Object);

        Assert.Throws<ArgumentNullException>(() => injectable.Inject(null));
    }
}
