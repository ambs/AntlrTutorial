using System.Globalization;

namespace Logo2Svg.AST;

public class ValueParam : Parameter
{
    public readonly float FloatValue;
    public ValueParam(float floatValue) => FloatValue = floatValue;
    
    public override string ToString() => FloatValue.ToString(CultureInfo.InvariantCulture);

    public override float Value(Turtle turtle) => FloatValue;
}
