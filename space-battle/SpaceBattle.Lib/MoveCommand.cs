
namespace SpaceBattle.lib;

public class MoveCommand(IMovingObject moving) : ICommand
{
    readonly IMovingObject _moving = moving;

    public void Execute()
    {
        _moving.Position += _moving.Velocity;
    }
}
