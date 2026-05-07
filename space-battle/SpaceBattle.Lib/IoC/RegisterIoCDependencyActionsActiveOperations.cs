using App;
using System.Collections.Generic;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyActionsActiveOperations : SpaceBattle.lib.ICommand
{
    public void Execute()
    {
        var activeOperations = new Dictionary<object, SpaceBattle.lib.ICommand>();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.ActiveOperations", (object[] args) =>
        {
            return activeOperations;
        }).Execute();
    }
}
