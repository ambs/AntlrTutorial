using System.Globalization;
using System.Text.RegularExpressions;
using Logo2Svg.SVG;

namespace Logo2Svg.AST;

public class ColourNode : INode
{
    private Parameter _redExpr, _greenExpr, _blueExpr;
    private Colour _cssColour;
    private Parameter _id;

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
            if (!SVG.Colour.ColourNames.TryGetValue(possibleName.ToLowerInvariant(), out _cssColour))
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

    public Colour Colour(Turtle turtle)
    {
        if (_id is not null)
        {
            var i = (int) _id.Value(turtle);
            if (i is < 0 or > 15) throw new IndexOutOfRangeException($"Invalid palette index: {i}");
            return SVG.Colour.Palette[i];
        }

        if (_redExpr is not null)
        {
            var red = _redExpr.Value(turtle);
            var green = _greenExpr.Value(turtle);
            var blue = _blueExpr.Value(turtle);
            if (red is < 0 or > 99 || green is < 0 or > 99 || blue is < 0 or > 99)
                throw new IndexOutOfRangeException("Invalid LOGO colour intensity");
            const float factor = 255f / 99f;
            return new Colour((int) (factor * red), (int) (factor * green), (int) (factor * blue));
        }

        return _cssColour;
    }

    public void Execute(Turtle turtle) => throw new NotImplementedException();
}