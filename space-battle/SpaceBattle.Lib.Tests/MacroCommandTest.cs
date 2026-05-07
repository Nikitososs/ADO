
using Xunit;
using Moq;
using SpaceBattle.lib;

public class MacroCommandTest
{
    [Fact]
    public void MacroCommand_RunsAll()
    {
        var cmd1 = new Mock<ICommand>();
        var cmd2 = new Mock<ICommand>();
        var cmd3 = new Mock<ICommand>();

        ICommand macro = new MacroCommand([cmd1.Object, cmd2.Object, cmd3.Object]);

        macro.Execute();

        cmd1.Verify(c => c.Execute(), Times.Once());
        cmd2.Verify(c => c.Execute(), Times.Once());
        cmd3.Verify(c => c.Execute(), Times.Once());
    }

    [Fact]
    public void MacroCommand_ThrowsException()
    {
        var cmd1 = new Mock<ICommand>();
        var cmd2 = new Mock<ICommand>();
        cmd2.Setup(cmd => cmd.Execute()).Throws<Exception>();
        var cmd3 = new Mock<ICommand>();

        ICommand macro = new MacroCommand([cmd1.Object, cmd2.Object, cmd3.Object]);

        Assert.Throws<Exception>(() => macro.Execute());

        cmd1.Verify(c => c.Execute(), Times.Once());
        cmd2.Verify(c => c.Execute(), Times.Once());
        cmd3.Verify(c => c.Execute(), Times.Never());
    }
}
