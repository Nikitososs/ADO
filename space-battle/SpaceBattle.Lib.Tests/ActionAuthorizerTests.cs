using App;
using App.Scopes;
using SpaceBattle.lib;

public class ActionAuthorizerTests
{
    [Fact]
    public void CanPerform_ReturnsTrue_WhenActionGranted()
    {
        var authorizer = new PrefixTreeActionAuthorizer();
        authorizer.Grant("user-1", "ship-1", "Fire");

        Assert.True(authorizer.CanPerform("user-1", "ship-1", "Fire"));
    }

    [Fact]
    public void CanPerform_ReturnsFalse_WhenActionNotGranted()
    {
        var authorizer = new PrefixTreeActionAuthorizer();

        Assert.False(authorizer.CanPerform("user-1", "ship-1", "Move"));
    }

    [Fact]
    public void CanPerform_ReturnsFalse_WhenUserUnknown()
    {
        var authorizer = new PrefixTreeActionAuthorizer();
        authorizer.Grant("user-1", "ship-1", "Fire");

        Assert.False(authorizer.CanPerform("other-user", "ship-1", "Fire"));
    }

    [Fact]
    public void CanPerform_ReturnsFalse_WhenObjectUnknown()
    {
        var authorizer = new PrefixTreeActionAuthorizer();
        authorizer.Grant("user-1", "ship-1", "Fire");

        Assert.False(authorizer.CanPerform("user-1", "other-ship", "Fire"));
    }

    [Fact]
    public void Revoke_RemovesPermission()
    {
        var authorizer = new PrefixTreeActionAuthorizer();
        authorizer.Grant("user-1", "ship-1", "Fire");
        authorizer.Revoke("user-1", "ship-1", "Fire");

        Assert.False(authorizer.CanPerform("user-1", "ship-1", "Fire"));
    }

    [Fact]
    public void Revoke_OnMissingPermission_DoesNotThrow()
    {
        var authorizer = new PrefixTreeActionAuthorizer();

        var exception = Record.Exception(() => authorizer.Revoke("user-1", "ship-1", "Fire"));

        Assert.Null(exception);
    }

    [Fact]
    public void Grant_IsIdempotent_ForSameAction()
    {
        var authorizer = new PrefixTreeActionAuthorizer();
        authorizer.Grant("user-1", "ship-1", "Move");
        authorizer.Grant("user-1", "ship-1", "Move");

        Assert.True(authorizer.CanPerform("user-1", "ship-1", "Move"));
    }

    [Fact]
    public void CanPerform_SupportsDifferentActionsOnSameObject()
    {
        var authorizer = new PrefixTreeActionAuthorizer();
        authorizer.Grant("user-1", "ship-1", "Fire");
        authorizer.Grant("user-1", "ship-1", "Move");

        Assert.True(authorizer.CanPerform("user-1", "ship-1", "Fire"));
        Assert.True(authorizer.CanPerform("user-1", "ship-1", "Move"));
        Assert.False(authorizer.CanPerform("user-1", "ship-1", "Rotate"));
    }

    [Theory]
    [InlineData(null, "ship-1", "Fire")]
    [InlineData("user-1", null, "Fire")]
    [InlineData("user-1", "ship-1", null)]
    [InlineData("", "ship-1", "Fire")]
    [InlineData("user-1", "  ", "Fire")]
    public void CanPerform_Throws_WhenKeyIsNullOrWhiteSpace(string userId, string objectId, string action)
    {
        var authorizer = new PrefixTreeActionAuthorizer();

        Assert.Throws<ArgumentException>(() => authorizer.CanPerform(userId, objectId, action));
    }

    [Fact]
    public void RegisterIoCDependencyActionAuthorizer_ResolvesAuthorizer()
    {
        new InitCommand().Execute();
        var iocScope = Ioc.Resolve<object>("IoC.Scope.Create");
        Ioc.Resolve<App.ICommand>("IoC.Scope.Current.Set", iocScope).Execute();

        new RegisterIoCDependencyActionAuthorizer().Execute();

        var authorizer = Ioc.Resolve<IActionAuthorizer>("Authorization.Authorizer");
        Assert.IsType<PrefixTreeActionAuthorizer>(authorizer);

        ((PrefixTreeActionAuthorizer)authorizer).Grant("user-1", "ship-1", "Fire");
        Assert.True(authorizer.CanPerform("user-1", "ship-1", "Fire"));
    }
}
