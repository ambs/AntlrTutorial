namespace Logo2Svg.SVG;

public class Line
{
    public Point Pt1, Pt2;
    public Colour Colour;

    public Line(Point pt1, Point pt2, Colour c)
    {
        Pt1 = pt1;
        Pt2 = pt2;
        Colour = c;
    }

    public override string ToString()
    {
        var style = $"stroke:{Colour}; stroke-width:1";
        return $@"<line x1=""{Pt1.X}"" y1=""{Pt1.Y}"" x2=""{Pt2.X}"" y2=""{Pt2.Y}"" style=""{style}""/>";
    }
}