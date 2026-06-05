
namespace SpaceBattle.lib;

public interface ICollidable : IMovingObject, IGameObject
{
    ICollisionChecker CollisionChecker { get; }
}
