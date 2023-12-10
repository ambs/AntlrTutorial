namespace Logo2Svg.SVG;

/// <summary>
/// Represents a colour, composed of the RGB components (integers from 0 to 255).
/// </summary>
public class Colour
{
    private readonly int _red, _green, _blue;

    /// <summary>
    /// Constructor, sets a color given the integer RGB components.
    /// </summary>
    /// <param name="red">Red component.</param>
    /// <param name="green">Green component.</param>
    /// <param name="blue">Blue component.</param>
    public Colour(int red, int green, int blue)
    {
        _red = red;
        _green = green;
        _blue = blue;
    }

    /// <summary>
    /// Returns a string representation of this colour in SVG/CSS.
    /// </summary>
    /// <returns>Returns the CSS attribute to set the desired colour.</returns>
    public override string ToString() => $"rgb({_red},{_green},{_blue})";
}

