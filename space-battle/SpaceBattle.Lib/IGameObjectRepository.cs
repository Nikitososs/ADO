using System.Collections.Generic;

namespace SpaceBattle.lib;

public interface IGameObjectRepository
{
    void Add(string id, IGameObject gameObject);
    bool TryGet(string id, out IGameObject gameObject);
    bool Remove(string id);
}
