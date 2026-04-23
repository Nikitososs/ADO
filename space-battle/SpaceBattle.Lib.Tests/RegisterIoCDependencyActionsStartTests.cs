
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
        var operation = new Mock<SpaceBattle.lib.ICommand>();
        var receiver = new Mock<SpaceBattle.lib.ICommandReceiver>();
        var targetObject = new object();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Move", (object[] args) => operation.Object).Execute();

        var registration = new SpaceBattle.lib.RegisterIoCDependencyActionsStart();

        registration.Execute();

        IDictionary<string, object> order = new Dictionary<string, object>
        {
            { "target", targetObject },
            { "command", "Move" },
            { "receiver", receiver.Object },
        };

        var command = Ioc.Resolve<SpaceBattle.lib.ICommand>("Actions.Start", order);

        Assert.NotNull(command);
        Assert.IsType<SpaceBattle.lib.ActionStartCommand>(command);
    }

    [Fact]
    public void ActionStartCommand_ExecutesOperation_AndEnqueuesItself()
    {
        var operation = new Mock<SpaceBattle.lib.ICommand>();
        var receiver = new Mock<SpaceBattle.lib.ICommandReceiver>();
        var targetObject = new object();

        Ioc.Resolve<App.ICommand>("IoC.Register", "Move", (object[] args) => operation.Object).Execute();
        new SpaceBattle.lib.RegisterIoCDependencyActionsStart().Execute();

        IDictionary<string, object> order = new Dictionary<string, object>
        {
            { "target", targetObject },
            { "command", "Move" },
            { "receiver", receiver.Object },
        };

        var startCommand = Ioc.Resolve<SpaceBattle.lib.ICommand>("Actions.Start", order);
        startCommand.Execute();

        operation.Verify(op => op.Execute(), Times.Once);
        receiver.Verify(r => r.Receive(It.Is<SpaceBattle.lib.ICommand>(c => ReferenceEquals(c, startCommand))), Times.Once);
    }
}
