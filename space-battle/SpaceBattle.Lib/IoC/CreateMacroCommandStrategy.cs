
using App;

namespace SpaceBattle.lib;

public class CreateMacroCommandStrategy(string commandSpec)
{
    private readonly string _commandSpec = commandSpec;

    public ICommand Resolve(params object[] args)
    {
        var commandNames = Ioc.Resolve<IEnumerable<string>>(_commandSpec);
        var commands = commandNames
            .Select(name => Ioc.Resolve<ICommand>(name, args))
            .ToArray();
        return new MacroCommand(commands);
    }
}
