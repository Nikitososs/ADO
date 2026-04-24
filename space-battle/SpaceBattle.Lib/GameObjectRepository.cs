using System.Collections.Generic;
using System.Linq;

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

    public IReadOnlyCollection<IGameObject> GetAll()
    {
        return _gameObjects.Values.ToList();
    }
}
