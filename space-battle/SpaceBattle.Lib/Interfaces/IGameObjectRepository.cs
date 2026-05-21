
namespace SpaceBattle.lib;

public interface IGameObjectRepository
{
    void Add(string objectId, object gameObject);

    object Get(string objectId);

    void Remove(string objectId);
}
