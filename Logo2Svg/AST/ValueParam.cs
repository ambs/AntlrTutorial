using System.Globalization;

namespace Logo2Svg.AST;

public class ValueParam : Parameter
{
    private readonly float _value;
    public ValueParam(float value) =>_value = value;
    
    public override string ToString() => _value.ToString(CultureInfo.InvariantCulture);

    public override float Value(Turtle turtle) => _value;
}
