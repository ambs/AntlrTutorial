using Logo2Svg.Language;
using Logo2Svg.SVG;

namespace Logo2Svg.AST;

/// <summary>
/// AST Node representing a command.
/// </summary>
public class Command : INode
{
    /// <summary>
    /// The command ID (borrowed from LogoLexer token list).
    /// </summary>
    public int Id { get; }
    
    /// <summary>
    /// List of command parameters and code blocks.
    /// </summary>
    public readonly List<INode> Params;
    
    public string Name { get; }

    private Parameter Parameter(int index) => Parameter<Parameter>(index);
    

    /// <summary>
    /// Constructor for a parameter.
    /// </summary>
    /// <param name="id">The parameter ID.</param>
    /// <param name="command">The command name (string) used for stringification.</param>
    /// <param name="params">A variable number of parameters or child nodes.</param>
    public Command(int id, string command, params INode[] @params)
    {
        Id = id;
        Name = command;
        Params = @params is null ? null : new List<INode>(@params);
    }

    /// <summary>
    /// Stringification of the command.
    /// </summary>
    /// <returns>A string representing the command.</returns>
    public override string ToString()
    {
        var parameters = Params is null ? "" : string.Join(",", Params);
        return $"{Name}({parameters})";
    }

    /// <summary>
    /// Generic accessor for a command parameter.
    /// </summary>
    /// <param name="index">Index of the desired parameter.</param>
    /// <typeparam name="T">Type of the parameter.</typeparam>
    /// <returns>The desired parameter.</returns>
    /// <exception cref="IndexOutOfRangeException">The supplied index is for an non-existent parameter.</exception>
    /// <exception cref="InvalidCastException">The requested type is not the type of the referred parameter.</exception>
    public T Parameter<T>(int index)
    {
        if (index < 0 || index >= Params.Count) throw new IndexOutOfRangeException("Parameter out of bounds");
        if (Params[index] is not T p) throw new InvalidCastException("Invalid parameter type");
        return p;
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="turtle">The Turtle information.</param>
    /// <exception cref="Exception">The ID of the command is invalid.</exception>
    public void Execute(Turtle turtle)
    {
        switch (Id)
        {
            case LogoLexer.SetPenColor:
                turtle.Colour = Parameter<ColourNode>(0).Colour(turtle);
                break;
            case LogoLexer.SetPalette:
            {
                var pos = (int) Parameter(0).Value(turtle);
                if (pos is < 0 or > 15) throw new IndexOutOfRangeException();
                Colour.Palette[pos] = Parameter<ColourNode>(1).Colour(turtle);
                break;
            }
            case LogoLexer.PenDown:
                turtle.IsDrawing = true;
                break;
            case LogoLexer.PenUp:
                turtle.IsDrawing = false;
                break;
            
            case LogoLexer.SetPenSize:
                turtle.Width = (int) Parameter(0).Value(turtle);
                break;
            case LogoLexer.If:
            {
                var condition = Parameter(0).Value(turtle).AsBool();
                if (condition) Parameter<CommandBlock>(1).Execute(turtle);
                break;
            }
            case LogoLexer.IfElse:
            {
                var condition = Parameter(0).Value(turtle).AsBool();
                if (condition) Parameter<CommandBlock>(1).Execute(turtle);
                else Parameter<CommandBlock>(2).Execute(turtle);
                break;
            }
            case LogoLexer.Forever:
                while (!turtle.IsExiting) Parameter<CommandBlock>(0).Execute(turtle);
                break;
            case LogoLexer.Repeat:
            {
                var times = (int) Parameter(0).Value(turtle);
                for (var i = 0; i < times && !turtle.IsExiting; i++)
                    Parameter<CommandBlock>(1).Execute(turtle);
                break;
            }
            case LogoLexer.Bye:
                turtle.Exiting();
                break;
            case LogoLexer.Show:
            {
                var value = Parameter(0).Value(turtle);
                Console.WriteLine(value);
                break;
            }
            case LogoLexer.Make:
            {
                turtle.DefineVariable(Parameter<VarName>(0).Name, Parameter(1).Value(turtle));
                break;
            }
            case LogoLexer.Forward:
            {
                var value = Parameter(0).Value(turtle);
                var pos = turtle.Position;
                var target = new Point(pos.X + MathF.Cos(turtle.Rotation) * value,
                    pos.Y - MathF.Sin(turtle.Rotation) * value);
                if (turtle.IsDrawing) turtle.AddLine(pos, target);
                turtle.Position = target;
                break;
            }
            case LogoLexer.Back:
            {
                var value = Parameter(0).Value(turtle);
                var pos = turtle.Position;
                var target = new Point(pos.X + MathF.Cos(turtle.Rotation) * value,
                    pos.Y + MathF.Sin(turtle.Rotation) * value);
                if (turtle.IsDrawing) turtle.AddLine(pos, target);
                turtle.Position = target;
                break;
            }
            case LogoLexer.Right:
            {
                var value = Parameter(0).Value(turtle);
                turtle.Rotation -= value * Turtle.ToRadians;
                break;
            }
            case LogoLexer.Left:
            {
                var value = Parameter(0).Value(turtle);
                turtle.Rotation += value * Turtle.ToRadians;
                break;
            }

            case LogoLexer.Home:
                turtle.Reset();
                break;
            
            case LogoLexer.SetXY:
            case LogoLexer.SetPos:
            {
                var target = Parameter<PointParam>(0).Point(turtle);
                if (turtle.IsDrawing) turtle.AddLine(turtle.Position, target);
                turtle.Position = target;
                break;
            }

            case LogoLexer.SetX:
            {
                var target = turtle.Position.Clone();
                target.X = Parameter(0).Value(turtle);
                if (turtle.IsDrawing) turtle.AddLine(turtle.Position, target);
                turtle.Position = target;
                break;
            }
            case LogoLexer.SetY:
            {
                var target = turtle.Position.Clone();
                target.Y = Parameter(0).Value(turtle);
                if (turtle.IsDrawing) turtle.AddLine(turtle.Position, target);
                turtle.Position = target;
                break;
            }
            case LogoLexer.SetH:
                turtle.Rotation = Parameter(0).Value(turtle);
                break;

            case LogoLexer.Arc:
                if (turtle.IsDrawing)
                {
                    var angle = Parameter(0).Value(turtle) * Turtle.ToRadians;
                    var radius = Parameter(1).Value(turtle);
                    turtle.AddArc(turtle.Position, turtle.Rotation, radius, angle);
                }
                break;
            
            default:
                throw new Exception($"Unknown command: {Id}-{Name}");
        }
    }
}
