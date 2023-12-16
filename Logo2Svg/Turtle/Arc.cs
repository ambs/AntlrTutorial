namespace Logo2Svg.SVG;

/// <summary>
/// A drawable arc.
/// </summary>
public class Arc : IDrawable
{
    private Point _center;
    private readonly Colour _colour;
    private readonly float _facing;
    private readonly float _radius;
    private readonly float _angle;
    private readonly int _width;

    /// <summary>
    /// The constructor for an Arc.
    /// </summary>
    /// <param name="center">Point referring to the center of the arc.</param>
    /// <param name="facing">Current angle the turtle is pointing to.</param>
    /// <param name="radius">The radius of the arc.</param>
    /// <param name="angle">The size of the arc: angle to be drawn.</param>
    /// <param name="colour">Colour of the line.</param>
    /// <param name="width">Width of the line.</param>
    public Arc(Point center, float facing, float radius, float angle, Colour colour, int width)
    {
        _center = center;
        _facing = facing;
        _radius = radius;
        _angle = angle;
        _colour = colour;
        _width = width;
    }

    /// <summary>
    /// Computes the points to draw the arc.
    /// </summary>
    /// <returns>A list of points.</returns>
    private List<Point> ComputePoints()
    {
        List<Point> pts = new();
        for (var alpha = _facing; alpha > _facing - _angle; alpha -= Turtle.ToRadians)
        {
            pts.Add(ComputePoint(alpha));
        }
        return pts;
    }
    
    /// <summary>
    /// Computes a specific point of the arc given an angle.
    /// </summary>
    /// <param name="angle">The requested angle.</param>
    /// <returns>The computed point.</returns>
    private Point ComputePoint(float angle)
        => new(_center.X + MathF.Cos(angle) * _radius, _center.Y - MathF.Sin(angle) * _radius);
    
    /// <summary>
    /// Returns a string representation of the arc in SVG.
    /// </summary>
    /// <returns>The SVG element representing the arc.</returns>
    public override string ToString()
    {
        var points = ComputePoints();
        var first = points.First();
        
        var path = $"{first.X} {first.Y}";
        path = points.Skip(1).Aggregate(path, (current, pt) => current + $" L {pt.X} {pt.Y}");
        return @$"<path fill=""none"" style=""stroke:{_colour}; stroke-width:{_width}"" d=""M {path}""/>";
    }

    /// <summary>
    /// Implementation of the <see>IDrawable</see> method <c>MinMaxCoordinates</c>.
    /// Computes a tuple of two points referring the minimum and maximum points used by the arc.
    /// </summary>
    /// <returns>A tuple with the two computed coordinates.</returns>
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

    /// <summary>
    /// Implementation of the <see>IDrawable</see> method <c>Displace</c>.
    /// Moves the arc accordingly with the referred displacement.
    /// </summary>
    /// <param name="displacement">Amount of displacement.</param>
    public void Displace(Point displacement) => _center += displacement;
}
