namespace SpaceBattle.lib;

public class FireCommand(
    ITorpedoFactory torpedoFactory,
    IGameObjectRepository gameObjectRepository,
    IShip ship,
    IEnumerable<IFireRule> fireRules
) : ICommand
{
    public void Execute()
    {
        var context = new FireContext(ship, torpedoFactory, gameObjectRepository);
        fireRules.ToList().ForEach(rule => rule.Apply(context));
    }
}
