using System.Collections.ObjectModel;
using Logo2Svg.Language;
using Logo2Svg.Turtle;

namespace Logo2Svg.AST.Nodes;

/// <summary>
/// Represents an expression node in the AST.
/// </summary>
public class Expression : Parameter
{
    /// <summary>
    /// The operator ID (borrowed from the LogoLexer token ID).
    /// </summary>
    public readonly int Op;
    
    /// <summary>
    /// The list of expression operands.
    /// </summary>
    public ReadOnlyCollection<Parameter> Parameters => _parameters.AsReadOnly();
    private readonly List<Parameter> _parameters;
    
    /// <summary>
    /// Expression constructor.
    /// </summary>
    /// <param name="op">The ID of the operator.</param>
    /// <param name="parameters">The list of operands.</param>
    public Expression(int op, params Parameter[] parameters)
    {
        Op = op;
        _parameters = new List<Parameter>(parameters);
    }

    /// <summary>
    /// Evaluates the expression, returning its evaluated value (a float).
    /// </summary>
    /// <param name="turtleState">The turtle information.</param>
    /// <returns>The result of evaluating the expression.</returns>
    /// <exception cref="DivideByZeroException">Thrown on division by zero.</exception>
    public override float Value(TurtleState turtleState)
    {
        var values = _parameters.Select(p => p.Value(turtleState)).ToArray();
        return Op switch
        {
            LogoLexer.True => true.AsFloat(),
            LogoLexer.False => false.AsFloat(),
            LogoLexer.And => values.Select(x => x.AsBool()).Aggregate(true, (a, b) => a && b).AsFloat(),
            LogoLexer.Xor => values.Select(x => x.AsBool()).Aggregate(false, (a, b) => a ^ b).AsFloat(),
            LogoLexer.Or => values.Select(x => x.AsBool()).Aggregate(false, (a, b) => a || b).AsFloat(),
            LogoLexer.Less => (values[0] < values[1]).AsFloat(), 
            LogoLexer.Greater => (values[0] > values[1]).AsFloat(),
            LogoLexer.LessEqual => (values[0] <= values[1]).AsFloat(),
            LogoLexer.GreaterEqual => (values[0] >= values[1]).AsFloat(),
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
            LogoLexer.Radsin => MathF.Sin(values[0] * TurtleState.ToRadians),
            LogoLexer.Radcos => MathF.Cos(values[0] * TurtleState.ToRadians),
            LogoLexer.Radtan => MathF.Tan(values[0] * TurtleState.ToRadians),
            LogoLexer.Arctan => values.Length == 2 ? 
                MathF.Atan2(values[0], values[1]) : MathF.Atan(values[0]),
            LogoLexer.Radarctan => values.Length == 2 ? 
                MathF.Atan2(values[0] * TurtleState.ToRadians, values[1] * TurtleState.ToRadians) :
                MathF.Atan(values[0] * TurtleState.ToRadians),
            _ => 0
        };
    }

    /// <summary>
    /// Stringification of the expression in a syntax similar to LOGO.
    /// </summary>
    /// <returns>The resulting string.</returns>
    public override string ToString()
    {
        var values = _parameters.Select(p => p.ToString()).ToArray();
        var @params = string.Join(" ", values.Select(v => $"({v})"));
        var op = Op switch {
            LogoLexer.True => "true",
            LogoLexer.False => "false",
            LogoLexer.And => "and",
            LogoLexer.Or => "or",
            LogoLexer.Xor => "xor",
            LogoLexer.Less => "less?",
            LogoLexer.Greater => "greater?",
            LogoLexer.LessEqual => "lessEqual?",
            LogoLexer.GreaterEqual => "greaterEqual?",
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