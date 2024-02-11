using System.Globalization;
using System.Text.RegularExpressions;
using Logo2Svg.Turtle;

namespace Logo2Svg.AST;

public class ColourNode : INode
{
    private readonly Parameter _redExpr, _greenExpr, _blueExpr;
    private readonly Colour _cssColour;
    private readonly Parameter _id;

    public ColourNode(string possibleName)
    {
        if (possibleName.StartsWith("#"))
        {
            if (!Regex.IsMatch(possibleName, "^#[0-9A-Fa-f]{6}$"))
                throw new Exception("Invalid CSS colour");
            
            var red = int.Parse(possibleName.Substring(1, 2), NumberStyles.HexNumber);
            var green = int.Parse(possibleName.Substring(3, 2), NumberStyles.HexNumber);
            var blue = int.Parse(possibleName.Substring(5, 2), NumberStyles.HexNumber);
            _cssColour = new Colour(red, green, blue);
        }
        else
        {
            if (!Turtle.Colour.ColourNames.TryGetValue(possibleName.ToLowerInvariant(), out _cssColour))
                throw new Exception("Invalid CSS colour name");
        }
    }

    public ColourNode(Parameter id) => _id = id;

    public ColourNode(Parameter redExpr, Parameter greenExpr, Parameter blueExpr)
    {
        _redExpr = redExpr;
        _greenExpr = greenExpr;
        _blueExpr = blueExpr;
    }

    public Colour Colour(TurtleState turtleState)
    {
        if (_id is not null)
        {
            var i = (int) _id.Value(turtleState);
            if (i is < 0 or > 15) throw new IndexOutOfRangeException($"Invalid palette index: {i}");
            return Turtle.Colour.Palette[i];
        }

        if (_redExpr is not null)
        {
            var red = _redExpr.Value(turtleState);
            var green = _greenExpr.Value(turtleState);
            var blue = _blueExpr.Value(turtleState);
            if (red is < 0 or > 99 || green is < 0 or > 99 || blue is < 0 or > 99)
                throw new IndexOutOfRangeException("Invalid LOGO colour intensity");
            const float factor = 255f / 99f;
            return new Colour((int) (factor * red), (int) (factor * green), (int) (factor * blue));
        }

        return _cssColour;
    }

    public void Execute(TurtleState turtleState) => throw new NotImplementedException();
}