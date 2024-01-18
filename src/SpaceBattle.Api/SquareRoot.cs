namespace SpaceBattle.Api;
public static class SquareRoot
{

    private const double e = 1e-5;

    public static double[] Solve(double a, double b, double c)
    {
        if (Math.Abs(a) < e)
        {
            throw new ArgumentException($"{a} не равно 0", nameof(a));
        }
        var discriminant = Math.Pow(b, 2) - 4 * a * c;
        if (discriminant < 0)
        {
            return [];
        }

        if (discriminant == 0)
        {
            var x1 = -b / (2 * a);
            return [x1];
        }
        else
        {
            var x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            var x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            return [x1, x2];
        }


    }
}