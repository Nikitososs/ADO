using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyIdGenerator : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Generators.Id",
            (object[] args) => new GuidIdGenerator()
        ).Execute();
    }
}
