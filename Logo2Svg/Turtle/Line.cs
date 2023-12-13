namespace Logo2Svg.SVG;

/// <summary>
/// Represents a SVG drawable line.
/// </summary>
public class Line : IDrawable
{
    private Point _pt1, _pt2;
    private readonly Colour _colour;
    private readonly int _width;

    /// <summary>
    /// Constructor, given two points and a colour.
    /// </summary>
    /// <param name="pt1">Source point.</param>
    /// <param name="pt2">Target point.</param>
    /// <param name="colour">Line colour.</param>
    /// <param name="width">Line width.</param>
    public Line(Point pt1, Point pt2, Colour colour, int width)
    {
        _pt1 = pt1;
        _pt2 = pt2;
        _colour = colour;
        _width = width;
    }

    /// <summary>
    /// Returns a SVG string representation of the line.
    /// </summary>
    /// <returns>SVG element representing the line.</returns>
    public override string ToString()
    {
        var style = $"stroke:{_colour}; stroke-width:{_width}";
        return $@"<line x1=""{_pt1.X}"" y1=""{_pt1.Y}"" x2=""{_pt2.X}"" y2=""{_pt2.Y}"" style=""{style}""/>";
    }

    /// <summary>
    /// Implementation of the <see>IDrawable</see> method <c>MinMaxCoordinates</c>.
    /// Computes a tuple of two points referring the minimum and maximum points used by the line.
    /// </summary>
    /// <returns>A tuple with the two computed coordinates.</returns>
    public (Point, Point) MinMaxCoordinates() => (
        new Point(MathF.Min(_pt1.X, _pt2.X), MathF.Min(_pt1.Y, _pt2.Y)), 
        new Point(MathF.Max(_pt1.X, _pt2.X), MathF.Max(_pt1.Y, _pt2.Y))
    );

    /// <summary>
    /// Implementation of the <see>IDrawable</see> method <c>Displace</c>.
    /// Moves the line accordingly with the referred displacement.
    /// </summary>
    /// <param name="displacement">Amount of displacement.</param>
    public void Displace(Point displacement)
    {
        _pt1 += displacement;
        _pt2 += displacement;
    }
}