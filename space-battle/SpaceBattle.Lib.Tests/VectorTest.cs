
using Xunit;
using SpaceBattle.lib;

public class VectorTests
{
    [Fact]
    public void Add_Opposite_ReturnsZeroVector()
    {
        var v1 = new Vector(1, -1, 2);
        var v2 = new Vector(-1, 1, -2);
        var expected = new Vector(0, 0, 0);

        var result = v1 + v2;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void Add_DifferentDimensions_Left_ThrowsException()
    {
        var v1 = new Vector(1, 2);
        var v2 = new Vector(1, 2, 3);

        Assert.Throws<ArgumentException>(() => v1 + v2);
    }

    [Fact]
    public void Add_DifferentDimensions_Right_ThrowsException()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2);

        Assert.Throws<ArgumentException>(() => v1 + v2);
    }

    [Fact]
    public void Equals_Correct_ReturnsTrue()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2, 3);

        Assert.True(v1.Equals(v2));
    }

    [Fact]
    public void Equality_Correct_ReturnsTrue()
    {
        var v1 = new Vector(1, 2, 3);
        var v2 = new Vector(1, 2, 3);

        Assert.True(v1 == v2);
    }

    [Fact]
    public void Equals_DifferentCoordinates_ReturnsFalse()
    {
        var v1 = new Vector(1, 2);
        var v2 = new Vector(3, 4);

        Assert.False(v1.Equals(v2));
    }

    [Fact]
    public void Inequality_DifferentCoordinates_ReturnsTrue()
    {
        var v1 = new Vector(1, 2);
        var v2 = new Vector(3, 4);

        Assert.True(v1 != v2);
    }

    [Fact]
    public void GetHashCode_Returns()
    {
        var v = new Vector(1, 2, 3);
        var hashCode = v.GetHashCode();

        Assert.NotEqual(0, hashCode);
    }


    [Fact]
    public void Equals_CompareWithNull_ReturnsFalse()
    {
        var v = new Vector(1, 1);
        Assert.False(v.Equals(null));
    }

    [Fact]
    public void GetHashCode_DifferentOrder_ReturnsDifferentHash()
    {
        var v1 = new Vector(1, 2);
        var v2 = new Vector(2, 1);

        Assert.NotEqual(v1.GetHashCode(), v2.GetHashCode());
    }
}
