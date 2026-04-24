using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyGame : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Game",
            (object[] args) => new Game(Ioc.Resolve<ICommand>("Commands.SimulationStep"))
        ).Execute();
    }
}
