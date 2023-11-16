using Logo2Svg.SVG;

namespace Logo2Svg.AST;

public class PointParam : IParameter
{
    public ValueParam X { get; }
    public ValueParam Y { get; }

    public Point Point => new Point(X.Value, Y.Value);
    
    public PointParam(ValueParam x, ValueParam y)
    {
        X = x;
        Y = y;
    }


    public void Execute(Turtle turtle) => throw new NotImplementedException();
    public override string ToString() => $"({X},{Y})";
}
