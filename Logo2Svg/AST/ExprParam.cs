using Logo2Svg.Language;

namespace Logo2Svg.AST;

public class ExprParam : Parameter
{
    private readonly int _op;
    private readonly List<Parameter> _parameters;

    public ExprParam(int op, params Parameter[] parameters)
    {
        _op = op;
        _parameters = new List<Parameter>(parameters);
    }
    
    public override float Value(Turtle turtle)
    {
        var values = _parameters.Select(p => p.Value(turtle)).ToArray();
        return _op switch
        {
            LogoLexer.Sum => values.Aggregate(0f, (a, b) => a + b),
            LogoLexer.Difference => values[0] - values[1],
            LogoLexer.Minus => - values[0],
            // possible exception
            LogoLexer.Quotient => values.Length == 1 ? 1 / values[0] : values[0] / values[1],  
            LogoLexer.Product => values.Aggregate(1f, (a,b) => a * b),
            LogoLexer.Power => MathF.Pow(values[0], values[1]),
            LogoLexer.Remainder => MathF.Abs(values[0] % values[1]) * MathF.Sign(values[0]),
            LogoLexer.Modulo  => MathF.Abs(values[0] % values[1]) * MathF.Sign(values[1]),
            LogoLexer.Abs => MathF.Abs(values[0]),
            LogoLexer.Int => MathF.Truncate(values[0]),
            LogoLexer.Round => MathF.Truncate(values[0] + 0.5f),
            LogoLexer.Sqrt => MathF.Sqrt(values[0]),
            LogoLexer.Exp => MathF.Exp(values[0]),
            LogoLexer.Ln => MathF.Log(values[0]),
            LogoLexer.Log10 => MathF.Log10(values[0]),
            LogoLexer.Sin => MathF.Sin(values[0]),
            LogoLexer.Cos => MathF.Cos(values[0]),
            LogoLexer.Tan => MathF.Tan(values[0]),
            LogoLexer.Radsin => MathF.Sin(values[0] * Turtle.ToRadians),
            LogoLexer.Radcos => MathF.Cos(values[0] * Turtle.ToRadians),
            LogoLexer.Radtan => MathF.Tan(values[0] * Turtle.ToRadians),
            LogoLexer.Arctan => values.Length == 2 ? 
                MathF.Atan2(values[0], values[1]) : MathF.Atan(values[0]),
            LogoLexer.Radarctan => values.Length == 2 ? 
                MathF.Atan2(values[0] * Turtle.ToRadians, values[1] * Turtle.ToRadians) :
                MathF.Atan(values[0] * Turtle.ToRadians),
            _ => 0
        };
    }
}