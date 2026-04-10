
namespace SpaceBattle.lib;

public interface IRotatableObject
{
    Angle Angle { get; set; }
    Angle AngularVelocity { get; }
}
