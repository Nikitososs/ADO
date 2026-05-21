using Moq;
using SpaceBattle.lib;

public class GameSchedulerQueueTests
{
    [Fact]
    public void ReceiveAndTake_WorksWithoutIoc()
    {
        var queue = new GameSchedulerQueue();
        var cmd = new Mock<SpaceBattle.lib.ICommand>();

        queue.Receive(cmd.Object);

        Assert.Equal(1, queue.Count);
        queue.Take().Execute();
        cmd.Verify(c => c.Execute(), Times.Once());
        Assert.Equal(0, queue.Count);
    }
}
