
namespace SpaceBattle.lib;

public class PrecomputedCollisionChecker : ICollisionChecker
{
    private readonly Dictionary<int, Dictionary<int, Dictionary<int, HashSet<int>>>> _tree;

    private PrecomputedCollisionChecker(Dictionary<int, Dictionary<int, Dictionary<int, HashSet<int>>>> tree) =>
        _tree = tree;

    public static PrecomputedCollisionChecker Load(string filePath)
    {
        var lines = File.ReadAllLines(filePath)
            .Select(line => line.Trim())
            .Where(line => line.Length > 0)
            .ToArray();

        var tree = new Dictionary<int, Dictionary<int, Dictionary<int, HashSet<int>>>>();

        lines
            .Select(ParseLine)
            .ToList()
            .ForEach(values => Add(tree, values[0], values[1], values[2], values[3]));

        return new PrecomputedCollisionChecker(tree);
    }

    public bool Collides(int x, int y, int dx, int dy) =>
        _tree.TryGetValue(x, out var byY)
        && byY.TryGetValue(y, out var byDx)
        && byDx.TryGetValue(dx, out var velocities)
        && velocities.Contains(dy);

    public static bool WillCollide(ICollidable reference, ICollidable target)
    {
        var position = target.Position - reference.Position;
        var velocity = target.Velocity - reference.Velocity;

        return target.CollisionChecker.Collides(
            position[0],
            position[1],
            velocity[0],
            velocity[1]);
    }

    private static int[] ParseLine(string line)
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 4)
            throw new InvalidDataException($"Line must contain four integers: '{line}'.");

        return parts.Select(int.Parse).ToArray();
    }

    private static void Add(
        Dictionary<int, Dictionary<int, Dictionary<int, HashSet<int>>>> tree,
        int x,
        int y,
        int dx,
        int dy)
    {
        if (!tree.TryGetValue(x, out var byY))
        {
            byY = new Dictionary<int, Dictionary<int, HashSet<int>>>();
            tree[x] = byY;
        }

        if (!byY.TryGetValue(y, out var byDx))
        {
            byDx = new Dictionary<int, HashSet<int>>();
            byY[y] = byDx;
        }

        if (!byDx.TryGetValue(dx, out var velocities))
        {
            velocities = new HashSet<int>();
            byDx[dx] = velocities;
        }

        velocities.Add(dy);
    }
}
