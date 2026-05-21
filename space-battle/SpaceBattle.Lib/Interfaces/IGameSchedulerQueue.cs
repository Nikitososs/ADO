
namespace SpaceBattle.lib;

public interface IGameSchedulerQueue : IQueue, ICommandReceiver, IGame
{
    ICommand Take();
}
