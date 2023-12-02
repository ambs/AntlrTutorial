namespace Logo2Svg.AST;

public class VarName : Parameter
{
    private string _varName;

    public VarName(string varName)
    {
        _varName = varName;
    }
    public override float Value(Turtle turtle)
    {
        return turtle.RetrieveVariable(_varName, out var expr) ? 
            expr.Value(turtle) : 0f;
    }

    public override string ToString() => $@"""{_varName}";
}