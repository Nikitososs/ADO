
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyTorpedo : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Generators.Id",
            (object[] _) => new GuidIdGenerator()
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Torpedo.Factory",
            (object[] _) =>
            {
                var idGenerator = Ioc.Resolve<IIdGenerator>("Generators.Id");
                return new TorpedoFactory(idGenerator);
            }
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Adapters.IMovingObject",
            (object[] args) =>
            {
                if (args[0] is IMovingObject movingObject)
                    return movingObject;

                throw new InvalidOperationException(
                    $"Object of type {args[0]?.GetType().Name ?? "null"} does not implement {nameof(IMovingObject)}.");
            }
        ).Execute();
    }
}
