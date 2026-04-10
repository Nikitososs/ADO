
namespace SpaceBattle.lib;

public class Angle
{
    public static readonly int Denominator = 8;

    private int _numerator;
    public int Numerator => _numerator;

    public Angle(int numerator)
    {
        _numerator = (numerator % Denominator + Denominator) % Denominator;
    }

    public static Angle operator +(Angle a, Angle b)
    {
        return new Angle(a.Numerator + b.Numerator);
    }

    public static implicit operator double(Angle angle)
    {
        return 2 * Math.PI * angle.Numerator / Denominator;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Angle other)
            return Numerator == other.Numerator;
        return false;
    }

    public override int GetHashCode()
    {
        return Numerator.GetHashCode();
    }

    public static bool operator ==(Angle a, Angle b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;
        return a.Equals(b);
    }

    public static bool operator !=(Angle a, Angle b)
    {
        return !(a == b);
    }
}
