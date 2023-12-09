namespace Logo2Svg.SVG;

public class Point
{
    public float X, Y;

    public Point(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static Point operator +(Point p1, Point p2) => new (p1.X + p2.X, p1.Y + p2.Y);

    public Point Abs() => new(MathF.Abs(X), MathF.Abs(Y));

    public Point Clone() => new(X, Y);
}