
using App;
using App.Scopes;
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
}
