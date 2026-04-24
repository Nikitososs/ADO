using System.Collections.Generic;
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencySimulationStep : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Specs.Simulation",
            (object[] args) => new List<string> { "Commands.MoveTorpedoes" }
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.SimulationStep",
            (object[] args) => new CreateMacroCommandStrategy("Specs.Simulation").Resolve()
        ).Execute();
    }
}
