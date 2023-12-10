namespace Logo2Svg.SVG;

/// <summary>
/// The drawable canvas. Consists of a list of drawable elements.
/// Supports all the <c>List</c> methods.
/// </summary>
public class Canvas : List<IDrawable>
{
    /// <summary>
    /// Converts the canvas to a string in SVG format.
    /// </summary>
    /// <returns>The content of a SVG file with the canvas contents.</returns>
    public override string ToString()
    {
        var min = new Point(0, 0);
        var max = new Point(0, 0);
        foreach (var (iMin, iMax) in this.Select(drawable => drawable.MinMaxCoordinates()))
        {
            if (iMin.X < min.X) min.X = iMin.X;
            if (iMin.Y < min.Y) min.Y = iMin.Y;
            
            if (iMax.X > max.X) max.X = iMax.X;
            if (iMax.Y > max.Y) max.Y = iMax.Y;
        }

        var displacement = min.Abs();
        ForEach(item => item.Displace(displacement));
        max += displacement;
        var lines = string.Join("\n", this);
        return $@"<svg width=""{(int)max.X}"" height=""{(int)max.Y}"">{lines}</svg>";
    }
}