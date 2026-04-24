namespace SpaceBattle.lib;

public interface ITorpedoFactory
{
    ITorpedo Create(IShip ship);
}
