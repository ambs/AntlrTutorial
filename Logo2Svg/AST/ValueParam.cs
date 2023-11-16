using System.Globalization;

namespace Logo2Svg.AST;

public class ValueParam : IParameter
{
    public readonly float Value;

    public ValueParam(float value) => Value = value;
    
    public void Execute(Turtle turtle) =>  throw new NotImplementedException();

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}
