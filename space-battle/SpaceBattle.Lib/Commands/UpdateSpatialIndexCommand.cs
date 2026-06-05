
namespace SpaceBattle.lib;

public class UpdateSpatialIndexCommand(ICollidable moving, ISpatialIndex spatialIndex) : ICommand
{
    public void Execute()
    {
        var newPosition = moving.Position;
        var oldPosition = newPosition - moving.Velocity;
        spatialIndex.Update(moving.Id, oldPosition, newPosition);
    }
}
