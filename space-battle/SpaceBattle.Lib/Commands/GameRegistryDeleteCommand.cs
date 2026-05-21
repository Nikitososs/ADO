
namespace SpaceBattle.lib;

public class GameRegistryDeleteCommand(IGameObjectRepository repository, string objectId) : ICommand
{
    public void Execute()
    {
        repository.Remove(objectId);
    }
}
