namespace Logo2Svg.SVG;

public class Arc : IDrawable
{
    private Point _center;
    private readonly Colour _colour;
    private readonly float _facing;
    private readonly float _radius;
    private readonly float _angle;

    public Arc(Point center, float facing, float radius, float angle, Colour colour)
    {
        _center = center;
        _facing = facing;
        _radius = radius;
        _angle = angle;
        _colour = colour;
    }

    private List<Point> ComputePoints()
    {
        List<Point> pts = new();
        for (var alpha = _facing; alpha > _facing - _angle; alpha -= Turtle.ToRadians)
        {
            pts.Add(ComputePoint(alpha));
        }
        return pts;
    }
    
    private Point ComputePoint(float angle)
        => new(_center.X + MathF.Cos(angle) * _radius, _center.Y - MathF.Sin(angle) * _radius);
    
    public override string ToString()
    {
        var points = ComputePoints();
        var first = points.First();
        
        var path = $"{first.X} {first.Y}";
        path = points.Skip(1).Aggregate(path, (current, pt) => current + $" L {pt.X} {pt.Y}");
        return @$"<path fill=""none"" style=""stroke:{_colour}"" d=""M {path}""/>";
    }

    public (Point, Point) MinMaxCoordinates()
    {
        var points = ComputePoints();
        var min = points.First().Clone();
        var max = points.First().Clone();
        foreach (var pt in points.Skip(1))
        {
            if (pt.X < min.X) min.X = pt.X;
            if (pt.Y < min.Y) min.Y = pt.Y;
            if (pt.X > max.X) max.X = pt.X;
            if (pt.Y > max.Y) max.Y = pt.Y;
        }
        return (min, max);
    }

    public void Displace(Point displacement)
    {
        _center += displacement;
    }
}
