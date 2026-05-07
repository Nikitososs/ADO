
namespace SpaceBattle.lib;

public class CommandInjectableCommand(ICommand? cmd) : ICommand, ICommandInjectable
{
    private ICommand? _command = cmd;
    public void Execute()
    {
        if (_command == null) throw new InvalidOperationException("Команда не внедрена.");
        _command.Execute();
    }

    public void Inject(ICommand? command)
    {
        _command = command ?? throw new ArgumentNullException(nameof(command));
    }
}
