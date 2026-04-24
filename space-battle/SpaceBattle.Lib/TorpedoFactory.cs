namespace SpaceBattle.lib;

public class TorpedoFactory : ITorpedoFactory
{
    public ITorpedo Create(IShip ship)
    {
        return new Torpedo(ship.Position, ship.Direction);
    }
}
