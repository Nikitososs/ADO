
using App;
using System.Collections.Generic;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyActionsStop : SpaceBattle.lib.ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.StopCommand", (object[] args) =>
        {
            return new ActionStopCommand(args);
        }).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.Stop", (object[] args) =>
        {
            var order = (IDictionary<string, object>)args[0];
            var target = order["target"];

            return Ioc.Resolve<SpaceBattle.lib.ICommand>("Actions.StopCommand", target);
        }).Execute();
    }
}
