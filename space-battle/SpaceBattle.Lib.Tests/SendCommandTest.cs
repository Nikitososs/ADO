
using Xunit;
using Moq;
using App;
using SpaceBattle.lib;
using App.Scopes;

public class SendCommandTest
{
    [Fact]
    public void SendCommandTransfersCommandToReceiver()
    {
        var cmd = new Mock<SpaceBattle.lib.ICommand>();
        var receiver = new Mock<ICommandReceiver>();
        var sendCommand = new SendCommand(cmd.Object, receiver.Object);

        sendCommand.Execute();
        receiver.Verify(r => r.Receive(cmd.Object), Times.Once);
    }

    [Fact]
    public void ReceiverCantAcceptCommandThrowsException()
    {
        var cmd = new Mock<SpaceBattle.lib.ICommand>();
        var receiver = new Mock<ICommandReceiver>();
        receiver.Setup(r => r.Receive(It.IsAny<SpaceBattle.lib.ICommand>())).Throws<Exception>();

        var sendCommand = new SendCommand(cmd.Object, receiver.Object);

        Assert.Throws<Exception>(() => sendCommand.Execute());
    }
}

public class RegisterIoCDependencySendCommandTest
{
    public RegisterIoCDependencySendCommandTest()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void General()
    {
        var cmd = new Mock<SpaceBattle.lib.ICommand>();
        var receiver = new Mock<ICommandReceiver>();

        new RegisterIoCDependencySendCommand().Execute();

        var sendCommand = Ioc.Resolve<SpaceBattle.lib.ICommand>("Commands.SendCommand", [cmd.Object, receiver.Object]);
        sendCommand.Execute();

        receiver.Verify(r => r.Receive(cmd.Object), Times.Once);
    }
}