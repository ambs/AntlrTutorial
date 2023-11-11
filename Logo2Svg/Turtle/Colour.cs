namespace Logo2Svg.SVG;

public class Colour
{
    public int Red, Green, Blue;

    public Colour(int r, int g, int b)
    {
        Red = r;
        Green = g;
        Blue = b;
    }

    public override string ToString() => $"rgb({Red},{Green},{Blue})";
}

