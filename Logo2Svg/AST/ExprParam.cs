using System.Collections.ObjectModel;
using Logo2Svg.Language;

namespace Logo2Svg.AST;

public class ExprParam : Parameter
{
    public readonly int Op;
    private readonly List<Parameter> _parameters;
    public ReadOnlyCollection<Parameter> Parameters => _parameters.AsReadOnly();

    public ExprParam(int op, params Parameter[] parameters)
    {
        Op = op;
        _parameters = new List<Parameter>(parameters);
    }

    public override float Value(Turtle turtle)
    {
        var values = _parameters.Select(p => p.Value(turtle)).ToArray();
        return Op switch
        {
            LogoLexer.True => true.AsFloat(),
            LogoLexer.False => false.AsFloat(),
            
            LogoLexer.Less => (values[0] < values[1]).AsFloat(), 
            LogoLexer.Greater => (values[0] > values[1]).AsFloat(),
            LogoLexer.LessEqual => (values[0] <= values[1]).AsFloat(),
            LogoLexer.GreaterEqual => (values[0] >= values[1]).AsFloat(),
            
            LogoLexer.And => values.Select(x => x.AsBool()).Aggregate(true, (a, b) => a && b).AsFloat(),
            LogoLexer.Xor => values.Select(x => x.AsBool()).Aggregate(false, (a, b) => a ^ b).AsFloat(),
            LogoLexer.Or => values.Select(x => x.AsBool()).Aggregate(false, (a, b) => a || b).AsFloat(),
            
            LogoLexer.Sum => values.Aggregate(0f, (a, b) => a + b),
            LogoLexer.Difference => values[0] - values[1],
            LogoLexer.Minus => - values[0],
            // possible exception
            LogoLexer.Quotient => values.Length == 1 ? 
                values[0] != 0 ? 1 / values[0] : throw new DivideByZeroException(): 
                values[1] != 0 ? values[0] / values[1] : throw new DivideByZeroException(),  
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

    public override string ToString()
    {
        var values = _parameters.Select(p => p.ToString()).ToArray();
        var @params = string.Join(" ", values.Select(v => $"({v})"));
        var op = Op switch {
            LogoLexer.True => "true",
            LogoLexer.False => "false",
            
            LogoLexer.Less => "less?",
            LogoLexer.Greater => "greater?",
            LogoLexer.LessEqual => "lessEqual?",
            LogoLexer.GreaterEqual => "greaterEqual?",
            
            LogoLexer.And => "and",
            LogoLexer.Or => "or",
            LogoLexer.Xor => "xor",
            
            LogoLexer.Sum => "sum",
            LogoLexer.Difference => "difference",
            LogoLexer.Minus => "minus",
            LogoLexer.Quotient => "quotient",  
            LogoLexer.Product => "product",
            LogoLexer.Power => "power",
            LogoLexer.Remainder => "remainder",
            LogoLexer.Modulo  => "modulo",
            LogoLexer.Abs => "abs",
            LogoLexer.Int => "int",
            LogoLexer.Round => "round",
            LogoLexer.Sqrt => "sqrt",
            LogoLexer.Exp => "exp",
            LogoLexer.Ln => "ln",
            LogoLexer.Log10 => "log10",
            LogoLexer.Sin => "sin",
            LogoLexer.Cos => "cos",
            LogoLexer.Tan => "tan",
            LogoLexer.Radsin => "radsin",
            LogoLexer.Radcos => "radcos",
            LogoLexer.Radtan => "radtan",
            LogoLexer.Arctan => "arctan",
            LogoLexer.Radarctan => "radarctan",
            _ => ""
        };
        return @params.Length > 0 ? $"({op} {@params})" : op;
    }
}