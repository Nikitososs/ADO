using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyGameObjectRepository : ICommand
{
    public void Execute()
    {
        var repository = new GameObjectRepository();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Repositories.GameObjects",
            (object[] args) => repository
        ).Execute();
    }
}
