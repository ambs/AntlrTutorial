namespace Logo2Svg.SVG;

/// <summary>
/// Interface of all SVG drawable elements. 
/// </summary>
public interface IDrawable
{
    /// <summary>
    /// Computes a tuple of two points referring the minimum and maximum points used by the drawable element.
    /// </summary>
    /// <returns>A tuple with the two computed coordinates.</returns>
    public (Point, Point) MinMaxCoordinates();
    
    /// <summary>
    /// Displaces (translates) a drawable element accordingly with the requested amount.
    /// </summary>
    /// <param name="displacement">Amount of displacement.</param>
    public void Displace(Point displacement);
}