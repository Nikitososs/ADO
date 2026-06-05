
namespace SpaceBattle.lib;

public interface ICollisionChecker
{
    bool Collides(int relativeX, int relativeY, int relativeDx, int relativeDy);
}
