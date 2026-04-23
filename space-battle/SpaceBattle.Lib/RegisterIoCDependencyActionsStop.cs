
using App;
using System.Collections.Generic;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyActionsStop : SpaceBattle.lib.ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.StopCommand", (object[] args) =>
        {
            return new ActionStopCommand(
                (IDictionary<object, SpaceBattle.lib.ICommand>)args[0],
                args[1]
            );
        }).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.Stop", (object[] args) =>
        {
            var order = (IDictionary<string, object>)args[0];
            var target = order["target"];
            var activeOperations = order.TryGetValue("activeOperations", out var activeOperationsObj)
                ? (IDictionary<object, SpaceBattle.lib.ICommand>)activeOperationsObj
                : Ioc.Resolve<IDictionary<object, SpaceBattle.lib.ICommand>>("Actions.ActiveOperations");

            return Ioc.Resolve<SpaceBattle.lib.ICommand>("Actions.StopCommand", activeOperations, target);
        }).Execute();
    }
}
