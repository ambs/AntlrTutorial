using Logo2Svg.AST;
using Logo2Svg.SVG;

namespace Logo2Svg;

public class Turtle
{
    private Canvas _canvas = new();
    public Point Position;
    public Colour Colour = new(255, 0, 0);

    private Dictionary<string, Parameter> _symbolTable = new();
    
    public const float ToRadians = MathF.PI / 180f; 
    private float _rotation;
    public float Rotation
    {
        get => _rotation;
        set => _rotation = Norm(value);
    }

    public Turtle()
    {
        Reset();
    }
    
    private static float Norm(float a) {
        while (a > 2 * MathF.PI) a -= 2f * MathF.PI;
        while (a < 0f) a += 2f * MathF.PI;
        return a;
    }

    public void AddLine(Point from, Point to)
    {
        _canvas.Add(new Line(from, to, Colour));
    }
    
    public void AddArc(Point turtlePosition, float turtleAngle, float radius, float angle)
    {
        _canvas.Add(new Arc(turtlePosition, turtleAngle, radius, angle, Colour));
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

    public void DefineVariable(string varName, Parameter value) => _symbolTable[varName] = value;

    public bool RetrieveVariable(string varName, out Parameter value) => _symbolTable.TryGetValue(varName, out value);
    
    public void Reset()
    {
        Position = new Point(100, 100);
        Rotation = MathF.PI / 2f;
    }

}