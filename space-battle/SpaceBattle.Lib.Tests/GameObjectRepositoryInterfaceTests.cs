using Moq;
using SpaceBattle.lib;

public class GameObjectRepositoryInterfaceTests
{
    [Fact]
    public void IGameObjectRepository_CanBeMocked_AddGetRemove()
    {
        var repo = new Mock<IGameObjectRepository>();
        var payload = new object();
        repo.Setup(r => r.Get("t1")).Returns(payload);

        repo.Object.Add("t1", payload);

        Assert.Same(payload, repo.Object.Get("t1"));
        repo.Object.Remove("t1");
        repo.Verify(r => r.Add("t1", payload), Times.Once());
        repo.Verify(r => r.Remove("t1"), Times.Once());
    }
}
