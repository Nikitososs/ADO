
using App;
using App.Scopes;
using Moq;
using System.Collections.Generic;
using Xunit;

public class RegisterIoCDependencyActionsStopTests
{
    public RegisterIoCDependencyActionsStopTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_ShouldRegisterActionsStop_AndResolveSuccessfully()
    {
        var activeOperations = new Dictionary<object, SpaceBattle.lib.ICommand>();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.ActiveOperations", (object[] args) => activeOperations).Execute();

        var registration = new SpaceBattle.lib.RegisterIoCDependencyActionsStop();

        registration.Execute();

        IDictionary<string, object> order = new Dictionary<string, object>
        {
            { "target", new object() }
        };

        var command = Ioc.Resolve<SpaceBattle.lib.ICommand>("Actions.Stop", order);

        Assert.NotNull(command);
        Assert.IsType<SpaceBattle.lib.ActionStopCommand>(command);
    }

    [Fact]
    public void ActionStopCommand_Execute_ShouldRemoveOperationFromActiveOperations()
    {
        var activeOperations = new Dictionary<object, SpaceBattle.lib.ICommand>();
        Ioc.Resolve<App.ICommand>("IoC.Register", "Actions.ActiveOperations", (object[] args) => activeOperations).Execute();

        var registration = new SpaceBattle.lib.RegisterIoCDependencyActionsStop();
        registration.Execute();

        var target = new object();
        var operation = new Mock<SpaceBattle.lib.ICommand>();
        activeOperations[target] = operation.Object;

        IDictionary<string, object> order = new Dictionary<string, object>
        {
            { "target", target }
        };

        var command = Ioc.Resolve<SpaceBattle.lib.ICommand>("Actions.Stop", order);
        command.Execute();

        Assert.False(activeOperations.ContainsKey(target));
    }
}
