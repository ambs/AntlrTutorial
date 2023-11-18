namespace Logo2Svg.AST;

public abstract class Parameter : INode
{
    public abstract float Value(Turtle turtle);
    public void Execute(Turtle turtle) => throw new NotImplementedException();
}
