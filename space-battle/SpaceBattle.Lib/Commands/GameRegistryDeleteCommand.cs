
namespace SpaceBattle.lib;

public class GameRegistryDeleteCommand(
    IGameObjectRepository repository,
    string objectId,
    ISpatialIndex? spatialIndex = null) : ICommand
{
    public void Execute()
    {
        if (spatialIndex != null
            && repository.Get(objectId) is ICollidable collidable)
        {
            spatialIndex.Remove(objectId, collidable.Position);
        }

        repository.Remove(objectId);
    }
}
