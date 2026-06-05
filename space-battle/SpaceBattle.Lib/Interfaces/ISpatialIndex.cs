
namespace SpaceBattle.lib;

public interface ISpatialIndex
{
    void Insert(string objectId, Vector position);

    void Update(string objectId, Vector oldPosition, Vector newPosition);

    void Remove(string objectId, Vector position);

    IEnumerable<string> QueryNearby(Vector position);
}
