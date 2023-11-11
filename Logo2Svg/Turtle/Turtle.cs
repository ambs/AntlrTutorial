using Logo2Svg.SVG;

namespace Logo2Svg;

public class Turtle
{
    private Canvas _canvas = new();
    public Point Position = new(100, 100);
    public Colour Colour = new(255, 0, 0);

    public const float ToRadians = MathF.PI / 180f; 
    private float _rotation = ToRadians * 90f;
    public float Rotation
    {
        get => _rotation;
        set => _rotation = Norm(value);
    }
    
    private static float Norm(float a) {
        while(a > 2*MathF.PI) a -= MathF.PI;
        while(a < 0f) a += MathF.PI;
        return a;
    }

    public void AddLine(Point from, Point to)
    {
        _canvas.Add(new Line(from, to, Colour));
    }

    public bool Save(string filename)
    {
        try {
            using var fs = new FileStream(filename, FileMode.Create);
            using var stream = new StreamWriter(fs);

            stream.Write(_canvas);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}