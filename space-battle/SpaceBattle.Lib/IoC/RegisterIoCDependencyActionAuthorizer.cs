
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyActionAuthorizer : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Authorization.Authorizer",
            (object[] _) => new PrefixTreeActionAuthorizer()
        ).Execute();
    }
}
