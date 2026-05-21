
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyActionAuthorizer : ICommand
{
    public void Execute()
    {
        var authorizer = new PrefixTreeActionAuthorizer();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Authorization.Authorizer",
            (object[] _) => authorizer
        ).Execute();
    }
}
