using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyTorpedoFactory : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Factories.TorpedoFactory",
            (object[] args) => new TorpedoFactory()
        ).Execute();
    }
}
