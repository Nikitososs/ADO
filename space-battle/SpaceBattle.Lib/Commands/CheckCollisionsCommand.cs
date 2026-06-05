
namespace SpaceBattle.lib;

public class CheckCollisionsCommand(
    ICollidable moving,
    ISpatialIndex spatialIndex,
    IGameObjectRepository repository) : ICommand
{
    public IReadOnlyList<ICollidable> CollidedWith { get; private set; } = [];

    public void Execute()
    {
        CollidedWith = spatialIndex
            .QueryNearby(moving.Position)
            .Where(id => id != moving.Id)
            .Select(repository.Get)
            .OfType<ICollidable>()
            .Where(other => PrecomputedCollisionChecker.WillCollide(moving, other))
            .ToList();

        if (CollidedWith.Count > 0)
            throw new InvalidOperationException("Collision detected.");
    }
}
