using System.Collections.Generic;

namespace SpaceBattle.lib;

public class GameObjectRepository : IGameObjectRepository
{
    private readonly Dictionary<string, IGameObject> _gameObjects = new();

    public void Add(string id, IGameObject gameObject)
    {
        _gameObjects[id] = gameObject;
    }

    public bool TryGet(string id, out IGameObject gameObject)
    {
        return _gameObjects.TryGetValue(id, out gameObject!);
    }

    public bool Remove(string id)
    {
        return _gameObjects.Remove(id);
    }
}
