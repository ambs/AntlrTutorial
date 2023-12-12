using Logo2Svg.SVG;

namespace Logo2Svg;

public class Turtle
{
    /// <summary>
    /// Current turtle position.
    /// </summary>
    public Point Position;
    
    /// <summary>
    /// Current turtle drawing color.
    /// </summary>
    public Colour Colour = new(255, 0, 0);

    /// <summary>
    /// Constant used to convert degrees into radians. 
    /// </summary>
    public const float ToRadians = MathF.PI / 180f; 
    
    /// <summary>
    /// Defines if the turtle is in exiting mode.
    /// </summary>
    public bool IsExiting { get; private set; }

    /// <summary>
    /// Rotation of the turtle (angle it is facing to)
    /// </summary>
    public float Rotation
    {
        get => _rotation;
        set => _rotation = Norm(value);
    }

    private float _rotation;
    private readonly Dictionary<string, float> _symbolTable = new();
    private readonly Canvas _canvas = new();

    /// <summary>
    /// Turtle constructor.
    /// </summary>
    public Turtle() => Reset();

    /// <summary>
    /// Sets turtle in exiting mode.
    /// </summary>
    public void Exiting() => IsExiting = true;

    /// <summary>
    /// Normalizes an angle, guaranteeing it is in the interval -2Pi, 2Pi.
    /// </summary>
    /// <param name="angle">The angle to be normalized.</param>
    /// <returns></returns>
    private static float Norm(float angle) {
        while (angle > 2 * MathF.PI) angle -= 2f * MathF.PI;
        while (angle < 0f) angle += 2f * MathF.PI;
        return angle;
    }

    /// <summary>
    /// Adds a line to the turtle canvas
    /// </summary>
    /// <param name="from">The source point.</param>
    /// <param name="to">The target point.</param>
    public void AddLine(Point from, Point to)
        => _canvas.Add(new Line(from, to, Colour));
    
    /// <summary>
    /// Adds an Arc to the turtle canvas
    /// </summary>
    /// <param name="turtlePosition">The turtle position (center of the arc).</param>
    /// <param name="turtleAngle">The turtle rotation (start of the arc).</param>
    /// <param name="radius">The radius of the arc.</param>
    /// <param name="angle">The length of the arc (in degrees).</param>
    public void AddArc(Point turtlePosition, float turtleAngle, float radius, float angle)
        => _canvas.Add(new Arc(turtlePosition, turtleAngle, radius, angle, Colour));
    

    /// <summary>
    /// Saves the turtle canvas in a file.
    /// </summary>
    /// <param name="filename">The filename to use.</param>
    /// <returns>True in case of success.</returns>
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

    /// <summary>
    /// Resets the turtle position and rotation.
    /// </summary>
    public void Reset()
    {
        Position = new Point(0, 0);
        Rotation = MathF.PI / 2f;
    }

    /// <summary>
    /// Defines a variable in the Symbol Table.
    /// </summary>
    /// <param name="varName">The variable name to define.</param>
    /// <param name="value">The variable's value.</param>
    public void DefineVariable(string varName, float value) => _symbolTable[varName] = value;

    /// <summary>
    /// Queries the Symbol Table for a variable.
    /// </summary>
    /// <param name="varName">The variable name to be queries.</param>
    /// <param name="value">The value of the variable, if it is defined.</param>
    /// <returns>A boolean stating if the variable was found in the symbol table.</returns>
    public bool RetrieveVariable(string varName, out float value) => _symbolTable.TryGetValue(varName, out value);

}