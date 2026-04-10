
namespace SpaceBattle.lib;

public interface ICommandReceiver
{
    void Receive(ICommand cmd);
}
