namespace SpaceBattle.lib;

public class Torpedo(Vector position, Vector velocity) : ITorpedo
{
    public Vector Position { get; set; } = position;
    public Vector Velocity { get; } = velocity;
}
