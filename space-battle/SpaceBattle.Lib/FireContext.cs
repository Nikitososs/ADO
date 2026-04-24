namespace SpaceBattle.lib;

public class FireContext(
    IShip ship,
    ITorpedoFactory torpedoFactory,
    IGameObjectRepository gameObjectRepository
)
{
    public IShip Ship { get; } = ship;
    public ITorpedoFactory TorpedoFactory { get; } = torpedoFactory;
    public IGameObjectRepository GameObjectRepository { get; } = gameObjectRepository;
    public ITorpedo? Projectile { get; set; }
    public string? ProjectileId { get; set; }
}
