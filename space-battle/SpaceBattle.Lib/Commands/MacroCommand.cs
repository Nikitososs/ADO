
namespace SpaceBattle.lib;

public class MacroCommand(ICommand[] cmds) : ICommand
{

    readonly ICommand[] _cmds = cmds;
    public void Execute()
    {
        _cmds.ToList().ForEach(cmd => cmd.Execute());
    }
}
