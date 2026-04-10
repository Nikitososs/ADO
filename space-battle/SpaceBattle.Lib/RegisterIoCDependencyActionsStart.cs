
using App;
using System.Collections.Generic;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyActionsStart : SpaceBattle.lib.ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.StartCommand", (object[] args) =>
        {
            return new ActionStartCommand(args);
        }).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.Start", (object[] args) =>
        {
            var order = (IDictionary<string, object>)args[0];
            var target = order["target"];
            var command = (string)order["command"];

            return Ioc.Resolve<SpaceBattle.lib.ICommand>("Actions.StartCommand", target, command);
        }).Execute();
    }
}
