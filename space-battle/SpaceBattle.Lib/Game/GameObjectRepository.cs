
namespace SpaceBattle.lib;

public class GameObjectRepository : IGameObjectRepository
{
    private readonly Dictionary<string, object> _gameObjects = new();

    public void Add(string objectId, object gameObject)
    {
        _gameObjects[objectId] = gameObject;
    }

    public object Get(string objectId)
    {
        return _gameObjects[objectId];
    }

    public void Remove(string objectId)
    {
        _gameObjects.Remove(objectId);
    }
}
