using Logo2Svg.Language;
using Logo2Svg.SVG;
using Logo2Svg.Turtle;

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
    /// <param name="turtleState">The Turtle information.</param>
    /// <exception cref="Exception">The ID of the command is invalid.</exception>
    public void Execute(TurtleState turtleState)
    {
        switch (Id)
        {
            case -1:
                if (turtleState.RetrieveMethod(Name, Params.Count, out var method))
                    method.Execute(turtleState, Params);
                else
                    throw new Exception("Command not defined");
                break;
            case LogoLexer.StopTk:
                throw new LogoStopException();
            case LogoLexer.To:
                turtleState.DefineMethod(Parameter<Method>(0));
                break;
            case LogoLexer.SetPenColor:
                turtleState.Colour = Parameter<ColourNode>(0).Colour(turtleState);
                break;
            case LogoLexer.SetPalette:
            {
                var pos = (int) Parameter(0).Value(turtleState);
                if (pos is < 0 or > 15) throw new IndexOutOfRangeException();
                Colour.Palette[pos] = Parameter<ColourNode>(1).Colour(turtleState);
                break;
            }
            case LogoLexer.PenDown:
                turtleState.IsDrawing = true;
                break;
            case LogoLexer.PenUp:
                turtleState.IsDrawing = false;
                break;
            
            case LogoLexer.SetPenSize:
                turtleState.Width = (int) Parameter(0).Value(turtleState);
                break;
            case LogoLexer.If:
            {
                var condition = Parameter(0).Value(turtleState).AsBool();
                if (condition) Parameter<CommandBlock>(1).Execute(turtleState);
                break;
            }
            case LogoLexer.IfElse:
            {
                var condition = Parameter(0).Value(turtleState).AsBool();
                if (condition) Parameter<CommandBlock>(1).Execute(turtleState);
                else Parameter<CommandBlock>(2).Execute(turtleState);
                break;
            }
            case LogoLexer.Forever:
                while (!turtleState.IsExiting) Parameter<CommandBlock>(0).Execute(turtleState);
                break;
            case LogoLexer.Repeat:
            {
                var times = (int) Parameter(0).Value(turtleState);
                for (var i = 0; i < times && !turtleState.IsExiting; i++)
                    Parameter<CommandBlock>(1).Execute(turtleState);
                break;
            }
            case LogoLexer.Bye:
                turtleState.Exiting();
                break;
            case LogoLexer.Show:
            {
                var value = Parameter(0).Value(turtleState);
                Console.WriteLine(value);
                break;
            }
            case LogoLexer.Make:
            {
                turtleState.DefineVariable(Parameter<VarName>(0).Name, Parameter(1).Value(turtleState));
                break;
            }
            case LogoLexer.Forward:
            {
                var value = Parameter(0).Value(turtleState);
                var pos = turtleState.Position;
                var target = new Point(pos.X + MathF.Cos(turtleState.Rotation) * value,
                    pos.Y - MathF.Sin(turtleState.Rotation) * value);
                if (turtleState.IsDrawing) turtleState.AddLine(pos, target);
                turtleState.Position = target;
                break;
            }
            case LogoLexer.Back:
            {
                var value = Parameter(0).Value(turtleState);
                var pos = turtleState.Position;
                var target = new Point(pos.X - MathF.Cos(turtleState.Rotation) * value,
                    pos.Y + MathF.Sin(turtleState.Rotation) * value);
                if (turtleState.IsDrawing) turtleState.AddLine(pos, target);
                turtleState.Position = target;
                break;
            }
            case LogoLexer.Right:
            {
                var value = Parameter(0).Value(turtleState);
                turtleState.Rotation -= value * TurtleState.ToRadians;
                break;
            }
            case LogoLexer.Left:
            {
                var value = Parameter(0).Value(turtleState);
                turtleState.Rotation += value * TurtleState.ToRadians;
                break;
            }

            case LogoLexer.Home:
                turtleState.Reset();
                break;
            
            case LogoLexer.SetXY:
            case LogoLexer.SetPos:
            {
                var target = Parameter<PointParam>(0).Point(turtleState);
                if (turtleState.IsDrawing) turtleState.AddLine(turtleState.Position, target);
                turtleState.Position = target;
                break;
            }

            case LogoLexer.SetX:
            {
                var target = turtleState.Position.Clone();
                target.X = Parameter(0).Value(turtleState);
                if (turtleState.IsDrawing) turtleState.AddLine(turtleState.Position, target);
                turtleState.Position = target;
                break;
            }
            case LogoLexer.SetY:
            {
                var target = turtleState.Position.Clone();
                target.Y = Parameter(0).Value(turtleState);
                if (turtleState.IsDrawing) turtleState.AddLine(turtleState.Position, target);
                turtleState.Position = target;
                break;
            }
            case LogoLexer.SetH:
                turtleState.Rotation = Parameter(0).Value(turtleState);
                break;

            case LogoLexer.Arc:
                if (turtleState.IsDrawing)
                {
                    var angle = Parameter(0).Value(turtleState) * TurtleState.ToRadians;
                    var radius = Parameter(1).Value(turtleState);
                    turtleState.AddArc(turtleState.Position, turtleState.Rotation, radius, angle);
                }
                break;
            
            default:
                throw new Exception($"Unknown command: {Id}-{Name}");
        }
    }
}

public class LogoStopException : Exception
{
}
