namespace Logo2Svg.SVG;
public interface IDrawable
{
    public (Point, Point) MinMaxCoordinates();
    public void Displace(Point displacement);
}