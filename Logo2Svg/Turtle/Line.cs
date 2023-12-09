namespace Logo2Svg.SVG;

public class Line : IDrawable
{
    private Point _pt1, _pt2;
    private readonly Colour _colour;

    public Line(Point pt1, Point pt2, Colour c)
    {
        _pt1 = pt1;
        _pt2 = pt2;
        _colour = c;
    }

    public override string ToString()
    {
        var style = $"stroke:{_colour}; stroke-width:1";
        return $@"<line x1=""{_pt1.X}"" y1=""{_pt1.Y}"" x2=""{_pt2.X}"" y2=""{_pt2.Y}"" style=""{style}""/>";
    }

    public (Point, Point) MinMaxCoordinates() => (
        new Point(MathF.Min(_pt1.X, _pt2.X), MathF.Min(_pt1.Y, _pt2.Y)), 
        new Point(MathF.Max(_pt1.X, _pt2.X), MathF.Max(_pt1.Y, _pt2.Y))
    );

    public void Displace(Point displacement)
    {
        _pt1 += displacement;
        _pt2 += displacement;
    }
}