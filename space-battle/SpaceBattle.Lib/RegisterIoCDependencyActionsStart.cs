
using App;
using System.Collections.Generic;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyActionsStart : SpaceBattle.lib.ICommand
{
    public void Execute()
    {
        new RegisterIoCDependencyMacroCommand().Execute();
        new RegisterIoCDependencySendCommand().Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.StartCommand", (object[] args) =>
        {
            return new ActionStartCommand((SpaceBattle.lib.ICommand)args[0], (ICommandReceiver)args[1]);
        }).Execute();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.Start", (object[] args) =>
        {
            var order = (IDictionary<string, object>)args[0];
            var target = order["target"];
            var command = (string)order["command"];
            var receiver = (ICommandReceiver)order["receiver"];
            var longRunningOperation = Ioc.Resolve<SpaceBattle.lib.ICommand>(command, target);

            return Ioc.Resolve<SpaceBattle.lib.ICommand>("Actions.StartCommand", longRunningOperation, receiver);
        }).Execute();
    }
}
