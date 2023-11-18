using Logo2Svg.SVG;

namespace Logo2Svg.AST;

public class PointParam : Parameter
{
    private readonly Parameter _x, _y;

    public Point Point(Turtle t) => new Point(_x.Value(t), _y.Value(t));
    
    public PointParam(Parameter x, Parameter y)
    {
        _x = x;
        _y = y;
    }
    
    public override float Value(Turtle turtle) => throw new NotImplementedException();
    public override string ToString() => $"({_x},{_y})";
}
