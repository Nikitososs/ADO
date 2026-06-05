
using App;

namespace SpaceBattle.lib;

public class RegisterIoCDependencyCollision : ICommand
{
    public void Execute()
    {
        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Collision.SpatialIndex",
            (object[] _) => new UniformGridSpatialIndex()
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Collision.TorpedoProfile",
            (object[] args) =>
            {
                var path = args.Length > 0
                    ? (string)args[0]
                    : Ioc.Resolve<string>("Collision.TorpedoProfile.Path");

                return PrecomputedCollisionChecker.Load(path);
            }
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Adapters.ICollidable",
            (object[] args) => (ICollidable)args[0]
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.CheckCollisions",
            (object[] args) =>
            {
                var collidable = Ioc.Resolve<ICollidable>("Adapters.ICollidable", args[0]);

                return new CheckCollisionsCommand(
                    collidable,
                    Ioc.Resolve<ISpatialIndex>("Collision.SpatialIndex"),
                    Ioc.Resolve<IGameObjectRepository>("Game.Registry.Repository"));
            }
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Commands.UpdateSpatialIndex",
            (object[] args) =>
            {
                var collidable = Ioc.Resolve<ICollidable>("Adapters.ICollidable", args[0]);

                return new UpdateSpatialIndexCommand(
                    collidable,
                    Ioc.Resolve<ISpatialIndex>("Collision.SpatialIndex"));
            }
        ).Execute();

        Ioc.Resolve<App.ICommand>(
            "IoC.Register",
            "Specs.Move",
            (object[] _) => new List<string>
            {
                "Commands.CheckCollisions",
                "Commands.Move",
                "Commands.UpdateSpatialIndex",
            }
        ).Execute();
    }
}
