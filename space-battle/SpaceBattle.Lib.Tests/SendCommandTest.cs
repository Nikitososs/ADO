
using Xunit;
using Moq;
using SpaceBattle.lib;

public class SendCommandTest
{
    [Fact]
    public void SendCommandTransfersCommandToReceiver()
    {
        var cmd = new Mock<ICommand>();
        var receiver = new Mock<ICommandReceiver>();
        var sendCommand = new SendCommand(cmd.Object, receiver.Object);

        sendCommand.Execute();
        receiver.Verify(r => r.Receive(cmd.Object), Times.Once);
    }

    [Fact]
    public void ReceiverCantAcceptCommandThrowsException()
    {
        var cmd = new Mock<ICommand>();
        var receiver = new Mock<ICommandReceiver>();
        receiver.Setup(r => r.Receive(It.IsAny<ICommand>())).Throws<Exception>();

        var sendCommand = new SendCommand(cmd.Object, receiver.Object);

        Assert.Throws<Exception>(() => sendCommand.Execute());
    }
}
