namespace Logo2Svg.SVG;

public class Arc : IDrawable
{
    private readonly Point _center;
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

    private string ComputePoint(float angle)
        => $"{_center.X + MathF.Cos(angle) * _radius} {_center.Y - MathF.Sin(angle) * _radius}";
    
    public override string ToString()
    {
        var path = ComputePoint(_facing);
        for (var alpha = _facing; alpha > _facing - _angle; alpha -= Turtle.ToRadians)
        {
            path += $" L {ComputePoint(alpha)}";
        }
        return @$"<path fill=""none"" style=""stroke:{_colour}"" d=""M {path}""/>";
    }
}
