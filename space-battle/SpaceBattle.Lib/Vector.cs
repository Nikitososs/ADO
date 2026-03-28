
using System.Linq;

namespace SpaceBattle.lib;

public class Vector
{
    public int[] Coordinates { get; }

    public Vector(params int[] coordinates)
    {
        Coordinates = coordinates;
    }

    public static Vector operator +(Vector a, Vector b)
    {
        if (a.Coordinates.Length != b.Coordinates.Length)
            throw new ArgumentException("Векторы должны иметь одинаковую размерность.");

        var result = a.Coordinates.Zip(b.Coordinates, (x, y) => x + y).ToArray();
        return new Vector(result);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Vector other)
            return Coordinates.SequenceEqual(other.Coordinates);
        return false;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var coord in Coordinates)
        {
            hash.Add(coord);
        }
        return hash.ToHashCode();
    }


    public static bool operator ==(Vector a, Vector b) => Equals(a, b);
    public static bool operator !=(Vector a, Vector b) => !Equals(a, b);
}
