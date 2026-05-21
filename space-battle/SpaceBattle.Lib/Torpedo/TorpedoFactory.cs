
namespace SpaceBattle.lib;

public class TorpedoFactory(IIdGenerator idGenerator) : ITorpedoFactory
{
    public ITorpedo Create(IShootable shooter, Vector initialVelocity)
    {
        ArgumentNullException.ThrowIfNull(shooter);
        ArgumentNullException.ThrowIfNull(initialVelocity);

        var id = idGenerator.NewId();
        return new Torpedo(id, shooter.Position.Clone(), initialVelocity);
    }
}
