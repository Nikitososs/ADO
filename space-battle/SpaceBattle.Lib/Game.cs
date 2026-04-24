namespace SpaceBattle.lib;

public class Game(ICommand simulationStep) : IGame
{
    private readonly ICommand _simulationStep = simulationStep;
    private readonly Queue<ICommand> _queue = new();

    public void Receive(ICommand cmd)
    {
        _queue.Enqueue(cmd);
    }

    public void Step()
    {
        while (_queue.Count > 0)
        {
            _queue.Dequeue().Execute();
        }

        _simulationStep.Execute();
    }
}
