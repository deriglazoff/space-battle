using SpaceBattle.Api;
using Xunit;

namespace SpaceBattle.Test;

public class SquareRootTest
{
    [InlineData(1, 0, 1)]
    [Theory]
    public void Solve_Empty(double a, double b, double c)
    {
        var act = SquareRoot.Solve(a, b, c);

        Assert.Empty(act);

    }

    [InlineData(1, 0, -1, 1, -1)]
    [Theory]
    public void Solve_TwoRoot(double a, double b, double c, double x1, double x2)
    {
        var act = SquareRoot.Solve(a, b, c);

        Assert.Equal(x1, act[0]);
        Assert.Equal(x2, act[1]);


    }
    [InlineData(1, 2, 1, -1)]
    [Theory]
    public void Solve_OneRoot(double a, double b, double c, double x1)
    {
        var act = SquareRoot.Solve(a, b, c);

        Assert.Equal(x1, act[0]);

    }
    [InlineData(1e-6, 5, 5)]
    [InlineData(0.0000001, 5, 5)]
    [InlineData(uint.MinValue, double.MaxValue, double.MaxValue)]
    [InlineData(null, null, null)]
    [Theory]
    public void Solve_Exception(double a, double b, double c)
    {
        Assert.Throws<ArgumentException>(() => SquareRoot.Solve(a, b, c));

    }
}