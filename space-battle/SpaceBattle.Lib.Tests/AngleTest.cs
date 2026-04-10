using SpaceBattle.Lib;
using Xunit;

namespace SpaceBattle.Tests;

public class AngleTests
{
    [Fact]
    public void AngleAddition_Correct()
    {
        var a = new Angle(5);
        var b = new Angle(7);

        var result = a + b;

        Assert.Equal(4, result.Numerator);
    }

    [Fact]
    public void AngleEquals_Correct()
    {
        var a = new Angle(15);
        var b = new Angle(23);

        var result = a.Equals(b);

        Assert.True(result);
    }

    [Fact]
    public void AngleOperatorEquals_Correct()
    {
        var a = new Angle(15);
        var b = new Angle(23);

        var result = (a == b);

        Assert.True(result);
    }

    [Fact]
    public void AngleEquality_Correct()
    {
        var a = new Angle(1);
        var b = new Angle(2);

        var result = a.Equals(b);

        Assert.False(result);
    }

    [Fact]
    public void AngleOperatorNotEquals_Correct()
    {
        var a = new Angle(1);
        var b = new Angle(2);

        var result = (a != b);

        Assert.True(result);
    }

    [Fact]
    public void Angle_HasHashCode()
    {
        var angle = new Angle(5);

        var hashCode = angle.GetHashCode();

        Assert.NotEqual(0, hashCode);
    }
}
