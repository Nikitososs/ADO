
namespace SpaceBattle.lib;

public class GameRegistryCreateCommand(
    IGameObjectRepository repository,
    string objectId,
    object gameObject,
    ISpatialIndex? spatialIndex = null) : ICommand
{
    public void Execute()
    {
        repository.Add(objectId, gameObject);

        if (spatialIndex != null && gameObject is ICollidable collidable)
            spatialIndex.Insert(objectId, collidable.Position);
    }
}
