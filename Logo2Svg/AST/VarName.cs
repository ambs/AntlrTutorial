namespace Logo2Svg.AST;

public class VarName : Parameter
{
    public readonly string Name;

    public VarName(string varName) => Name = varName;
    
    public override float Value(Turtle turtle) => turtle.RetrieveVariable(Name, out var expr) ? expr : 0f;

    public override string ToString() => $@"""{Name}";
}