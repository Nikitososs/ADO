
using App;
using App.Scopes;
using Moq;
using System.Collections.Generic;
using Xunit;

public class RegisterIoCDependencyActionsStartTests
{
    public RegisterIoCDependencyActionsStartTests()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();
    }

    [Fact]
    public void Execute_ShouldRegisterActionsStart_AndResolveSuccessfully()
    {
        var registration = new SpaceBattle.lib.RegisterIoCDependencyActionsStart();

        registration.Execute();

        IDictionary<string, object> order = new Dictionary<string, object>
        {
            { "target", new object() },
            { "command", "Move" }
        };

        var command = Ioc.Resolve<SpaceBattle.lib.ICommand>("Actions.Start", order);

        Assert.NotNull(command);
        Assert.IsType<SpaceBattle.lib.ActionStartCommand>(command);
    }
}
