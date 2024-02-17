using System.ComponentModel.DataAnnotations;

namespace SpaceBattle.Api;

public static class SquareRoot
{
    private const double epsilon = 1e-5;
    private record SolveValidate(double A, double B, double C) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Math.Abs(A) < epsilon)
            {
                yield return new ValidationResult($"{A} не равно 0");
            }
            foreach (var item in new[] { A, B, C })
            {
                if (double.IsNaN(item))
                {
                    yield return new ValidationResult($"{item} IsNaN");
                }
                if (double.IsInfinity(item))
                {
                    yield return new ValidationResult($"{item} IsInfinity");
                }
            }
        }
    }

    public static double[] Solve(double a,double b, double c)
    {
        var validateModel = new SolveValidate(a, b, c);
        Validator.ValidateObject(validateModel, new ValidationContext(validateModel));

        var discriminant = Math.Pow(b, 2) - 4 * a * c;

        if (discriminant > epsilon)
        {
            var x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            var x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
            return [x1, x2];
        }
        if (-epsilon <= discriminant && discriminant <= epsilon)
        {
            var x1 = -b / (2 * a);
            return [x1];
        }
        else
        {
            return [];
        }
    }
}