using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyFixedTokenFireAuthorizer(string expectedToken) : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Authorizers.Fire",
            (object[] args) => new FixedTokenFireAuthorizer(expectedToken)
        ).Execute();
    }
}
