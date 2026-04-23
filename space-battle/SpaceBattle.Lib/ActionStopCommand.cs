
using System.Collections.Generic;

namespace SpaceBattle.lib;

public class ActionStopCommand : SpaceBattle.lib.ICommand
{
    private readonly IDictionary<object, ICommand> _activeOperations;
    private readonly object _target;

    public ActionStopCommand(IDictionary<object, ICommand> activeOperations, object target)
    {
        _activeOperations = activeOperations;
        _target = target;
    }

    public void Execute()
    {
        _activeOperations.Remove(_target);
    }
}