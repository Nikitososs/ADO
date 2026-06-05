
namespace SpaceBattle.lib;

public class UniformGridSpatialIndex : ISpatialIndex
{
    private readonly Dictionary<(int X, int Y), HashSet<string>> _cells = new();

    public void Insert(string objectId, Vector position) =>
        Cell(position).Add(objectId);

    public void Update(string objectId, Vector oldPosition, Vector newPosition)
    {
        Cell(oldPosition).Remove(objectId);
        Cell(newPosition).Add(objectId);
    }

    public void Remove(string objectId, Vector position) =>
        Cell(position).Remove(objectId);

    public IEnumerable<string> QueryNearby(Vector position)
    {
        var (cx, cy) = (position[0], position[1]);

        return Enumerable.Range(-1, 3)
            .SelectMany(dx => Enumerable.Range(-1, 3).Select(dy => (cx + dx, cy + dy)))
            .Where(_cells.ContainsKey)
            .SelectMany(cell => _cells[cell])
            .Distinct(StringComparer.Ordinal);
    }

    private HashSet<string> Cell(Vector position)
    {
        var key = (position[0], position[1]);

        if (!_cells.TryGetValue(key, out var objects))
        {
            objects = new HashSet<string>(StringComparer.Ordinal);
            _cells[key] = objects;
        }

        return objects;
    }
}
