
namespace SpaceBattle.lib;

public interface IGameSchedulerQueue : ICommandReceiver
{
    int Count { get; }

    ICommand Take();
}
