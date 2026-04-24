using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyFireAuthorizer : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Authorizers.Fire",
            (object[] args) => new AllowAllFireAuthorizer()
        ).Execute();
    }
}
