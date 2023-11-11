namespace Logo2Svg.SVG;

public class Canvas : List<Line>
{
    public override string ToString()
    {
        var lines = string.Join("\n", this);
        return $@"<svg width=""200"" height=""200"">{lines}</svg>";
    }
}