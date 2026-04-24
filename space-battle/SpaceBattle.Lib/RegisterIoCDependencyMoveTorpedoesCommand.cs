using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyMoveTorpedoesCommand : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.MoveTorpedoes",
            (object[] args) =>
            {
                var repository = Ioc.Resolve<IGameObjectRepository>("Repositories.GameObjects");
                return new MoveTorpedoesCommand(repository);
            }
        ).Execute();
    }
}
