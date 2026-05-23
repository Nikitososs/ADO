using SpaceBattle.lib;

public class GameObjectRepositoryTests
{
    [Fact]
    public void AddGetRemove_WorksCorrectly()
    {
        var repository = new GameObjectRepository();
        var payload = new object();
        const string objectId = "ship-1";

        repository.Add(objectId, payload);

        Assert.Same(payload, repository.Get(objectId));

        repository.Remove(objectId);

        Assert.Throws<KeyNotFoundException>(() => repository.Get(objectId));
    }
}
