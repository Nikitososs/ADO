
namespace SpaceBattle.lib;

public class RotateCommand : ICommand
{
    private readonly IRotatableObject _rotatable;

    public RotateCommand(IRotatableObject rotatable)
    {
        _rotatable = rotatable;
    }

    public void Execute()
    {
        _rotatable.Angle += _rotatable.AngularVelocity;
    }
}
