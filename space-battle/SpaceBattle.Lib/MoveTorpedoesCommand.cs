using System.Linq;
using App;

namespace SpaceBattle.lib;

public class MoveTorpedoesCommand(IGameObjectRepository gameObjectRepository) : ICommand
{
    public void Execute()
    {
        gameObjectRepository
            .GetAll()
            .OfType<ITorpedo>()
            .ToList()
            .ForEach(torpedo =>
                Ioc.Resolve<ICommand>("Commands.Move", torpedo).Execute());
    }
}
