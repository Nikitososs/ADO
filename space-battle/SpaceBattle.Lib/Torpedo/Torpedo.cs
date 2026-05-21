
namespace SpaceBattle.lib;

public class Torpedo(string id, Vector position, Vector velocity) : ITorpedo, IMovingObject
{
    public string Id { get; } = id;

    public Vector Position { get; set; } = position;

    public Vector Velocity { get; } = velocity;
}
