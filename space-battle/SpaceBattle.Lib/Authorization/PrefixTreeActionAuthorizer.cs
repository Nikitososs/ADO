
namespace SpaceBattle.lib;

public class PrefixTreeActionAuthorizer : IActionAuthorizer
{
    private static readonly StringComparer KeyComparer = StringComparer.Ordinal;

    private readonly Dictionary<string, Dictionary<string, HashSet<string>>> _tree =
        new(KeyComparer);

    public void Grant(string userId, string objectId, string action)
    {
        ValidateKey(userId, nameof(userId));
        ValidateKey(objectId, nameof(objectId));
        ValidateKey(action, nameof(action));

        if (!_tree.TryGetValue(userId, out var objects))
        {
            objects = new Dictionary<string, HashSet<string>>(KeyComparer);
            _tree[userId] = objects;
        }

        if (!objects.TryGetValue(objectId, out var actions))
        {
            actions = new HashSet<string>(KeyComparer);
            objects[objectId] = actions;
        }

        actions.Add(action);
    }

    public void Revoke(string userId, string objectId, string action)
    {
        ValidateKey(userId, nameof(userId));
        ValidateKey(objectId, nameof(objectId));
        ValidateKey(action, nameof(action));

        if (!_tree.TryGetValue(userId, out var objects))
            return;

        if (!objects.TryGetValue(objectId, out var actions))
            return;

        actions.Remove(action);

        if (actions.Count == 0)
            objects.Remove(objectId);

        if (objects.Count == 0)
            _tree.Remove(userId);
    }

    public bool CanPerform(string userId, string objectId, string action)
    {
        ValidateKey(userId, nameof(userId));
        ValidateKey(objectId, nameof(objectId));
        ValidateKey(action, nameof(action));

        return _tree.TryGetValue(userId, out var objects)
            && objects.TryGetValue(objectId, out var actions)
            && actions.Contains(action);
    }

    private static void ValidateKey(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be null or empty.", paramName);
    }
}
