
namespace SpaceBattle.lib;

public class GameSchedulerQueue : IQueue, ICommandReceiver, IGame
{
    private readonly Queue<ICommand> _commands = new();

    public int Count => _commands.Count;

    public void Receive(ICommand cmd)
    {
        _commands.Enqueue(cmd);
    }

    public ICommand Take()
    {
        if (_commands.Count == 0)
            throw new InvalidOperationException("Scheduler queue is empty.");

        return _commands.Dequeue();
    }
}
