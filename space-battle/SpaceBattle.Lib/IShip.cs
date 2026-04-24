namespace SpaceBattle.lib;

public interface IShip : IGameObject
{
    Vector Position { get; }
    Vector Direction { get; }
}
