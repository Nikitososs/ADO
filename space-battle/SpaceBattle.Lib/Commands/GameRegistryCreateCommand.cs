
namespace SpaceBattle.lib;

public class GameRegistryCreateCommand(
    IGameObjectRepository repository,
    string objectId,
    object gameObject) : ICommand
{
    public void Execute()
    {
        repository.Add(objectId, gameObject);
    }
}
