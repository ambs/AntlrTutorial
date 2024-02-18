using Logo2Svg.SVG;
using Logo2Svg.Turtle;

namespace Logo2Svg.AST.Nodes;

/// <summary>
/// Point parameter.
/// </summary>
public class PointParam : Parameter
{
    private readonly Parameter _x, _y;

    /// <summary>
    /// Constructor of a new point parameter.
    /// </summary>
    /// <param name="x">The X parameter.</param>
    /// <param name="y">The Y parameter.</param>
    public PointParam(Parameter x, Parameter y)
    {
        _x = x;
        _y = y;
    }

    /// <summary>
    /// Evaluator of a parameter point.
    /// </summary>
    /// <param name="turtleState">The turtle information.</param>
    /// <returns>a new point with the point parameter values after evaluation</returns>
    public Point Point(TurtleState turtleState) => new(_x.Value(turtleState), _y.Value(turtleState));
    
    /// <summary>
    /// A point parameter can't be evaluated as a float value but rather as a point.
    /// <see cref="Point"/>
    /// </summary>
    /// <param name="turtleState">The turtle information.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException">This method can't be implemented.</exception>
    public override float Value(TurtleState turtleState) => throw new NotImplementedException();
    
    /// <summary>
    /// Stringification function.
    /// </summary>
    /// <returns>A string representation of the point</returns>
    public override string ToString() => $"[{_x} {_y}]";
}
