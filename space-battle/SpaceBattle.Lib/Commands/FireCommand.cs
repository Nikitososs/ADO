
using App;
using System.Collections.Generic;

namespace SpaceBattle.lib;

public class FireCommand(
    string userId,
    string shipObjectId,
    IShootable shooter,
    Vector initialVelocity,
    IActionAuthorizer authorizer,
    ITorpedoFactory torpedoFactory,
    ICommandReceiver receiver) : ICommand
{
    public const string FireAction = "Fire";

    public void Execute()
    {
        if (!authorizer.CanPerform(userId, shipObjectId, FireAction))
            throw new UnauthorizedAccessException(
                $"User '{userId}' is not allowed to perform '{FireAction}' on object '{shipObjectId}'.");

        var torpedo = torpedoFactory.Create(shooter, initialVelocity);

        Ioc.Resolve<ICommand>("Game.Registry.Create", torpedo.Id, torpedo).Execute();

        IDictionary<string, object> order = new Dictionary<string, object>
        {
            { "target", torpedo },
            { "command", "Commands.Move" },
            { "receiver", receiver },
        };

        Ioc.Resolve<ICommand>("Actions.Start", order).Execute();
    }
}
