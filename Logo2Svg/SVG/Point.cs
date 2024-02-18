namespace Logo2Svg.SVG;

/// <summary>
/// Abstraction of a point in a 2D coordinate system.
/// </summary>
public class Point
{
    public float X, Y;

    /// <summary>
    /// Point constructor
    /// </summary>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate.</param>
    public Point(float x, float y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// Overrides the sum operator. Sums the two points, attribute by attribute.
    /// </summary>
    /// <param name="p1">Sum first operand.</param>
    /// <param name="p2">Sum second operand.</param>
    /// <returns>A new point, with the result of the sum.</returns>
    public static Point operator +(Point p1, Point p2) => new (p1.X + p2.X, p1.Y + p2.Y);

    /// <summary>
    /// Computes the absolute value of the point (module value of both coordinates)
    /// </summary>
    /// <returns>A new point with the computed absolute value.</returns>
    public Point Abs() => new(MathF.Abs(X), MathF.Abs(Y));

    /// <summary>
    /// Creates a copy of the current point.
    /// </summary>
    /// <returns>A copy of the actual point.</returns>
    public Point Clone() => new(X, Y);
}