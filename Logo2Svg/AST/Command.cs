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
    
    private string Name { get; }

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
            {
                var colour = Parameter<INode>(0);
                switch (colour)
                {
                    case ColourNode colourNode:
                        turtle.Colour = colourNode.Colour(turtle);
                        break;
                    case ExprParam colourIndex:
                        var pos = (int) colourIndex.Value(turtle);
                        if (pos is < 0 or > 15) throw new IndexOutOfRangeException();
                        turtle.Colour = Colour.Palette[pos];
                        break;
                    case VarName colourName:
                        turtle.Colour = new ColourNode(colourName.Name).Colour(turtle);
                        break;
                    default:
                        throw new Exception("wrong parameter type to SetPenColor");
                }
                break;
            }
            case LogoLexer.SetPalette:
            {
                var pos = (int) Parameter(0).Value(turtle);
                var colour = Parameter<INode>(1);
                switch (colour)
                {
                    case ColourNode colourNode:
                        if (pos is < 0 or > 15) throw new IndexOutOfRangeException();
                        Colour.Palette[pos] = colourNode.Colour(turtle);
                        break;
                    case VarName namedColour:
                        Colour.Palette[pos] = new ColourNode(namedColour.Name).Colour(turtle);
                        break;
                    default:
                        throw new Exception("Invalid parameter type to SetPalette");
                }
                break;
            }
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
                turtle.AddLine(pos, target);
                turtle.Position = target;
                break;
            }
            case LogoLexer.Back:
            {
                var value = Parameter(0).Value(turtle);
                var pos = turtle.Position;
                var target = new Point(pos.X + MathF.Cos(turtle.Rotation) * value,
                    pos.Y + MathF.Sin(turtle.Rotation) * value);
                turtle.AddLine(pos, target);
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
                turtle.Position = Parameter<PointParam>(0).Point(turtle);
                break;

            case LogoLexer.SetX:
                turtle.Position.X = Parameter(0).Value(turtle);
                break;
            case LogoLexer.SetY:
                turtle.Position.Y = Parameter(0).Value(turtle);
                break;
            case LogoLexer.SetH:
                turtle.Rotation = Parameter(0).Value(turtle);
                break;

            case LogoLexer.Arc:
            {
                var angle = Parameter(0).Value(turtle) * Turtle.ToRadians;
                var radius = Parameter(1).Value(turtle);
                turtle.AddArc(turtle.Position, turtle.Rotation, radius, angle);
                break;
            }

            default:
                throw new Exception($"Unknown command: {Id}-{Name}");
        }
    }
}
