
namespace SpaceBattle.lib;

public interface ITorpedoFactory
{
    ITorpedo Create(IShootable shooter, Vector initialVelocity);
}
