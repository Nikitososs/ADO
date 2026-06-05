
namespace SpaceBattle.lib;

public class Ship(
    string id,
    Vector position,
    Vector velocity,
    Angle facing,
    ICollisionChecker collisionChecker) : ICollidable, IShootable
{
    public string Id { get; } = id;

    public Vector Position { get; set; } = position;

    public Vector Velocity { get; set; } = velocity;

    public Angle Facing { get; set; } = facing;

    public ICollisionChecker CollisionChecker { get; } = collisionChecker;
}
