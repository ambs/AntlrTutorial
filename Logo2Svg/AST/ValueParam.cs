using System.Globalization;
using Logo2Svg.Turtle;

namespace Logo2Svg.AST;

/// <summary>
/// A float value parameter.
/// </summary>
public class ValueParam : Parameter
{
    /// <summary>
    /// The real value of the parameter.
    /// </summary>
    public readonly float FloatValue;
    
    /// <summary>
    /// Constructor of the value parameter.
    /// </summary>
    /// <param name="floatValue">The parameter value.</param>
    public ValueParam(float floatValue) => FloatValue = floatValue;
    
    /// <summary>
    /// Stringification of the value parameter.
    /// </summary>
    /// <returns>A string representation of the float value.</returns>
    public override string ToString() => FloatValue.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Evaluation method, given a Turtle.
    /// </summary>
    /// <param name="turtleState">The turtle data.</param>
    /// <returns>The float value.</returns>
    public override float Value(TurtleState turtleState) => FloatValue;
}
