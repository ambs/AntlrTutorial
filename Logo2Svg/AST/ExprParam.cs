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
            _ => 0
        };
    }
}