using Logo2Svg.Language;
using Logo2Svg.SVG;

namespace Logo2Svg.AST
{
    public class Command : INode
    {
        public string Name { get; }
        public int Id { get;  }
        public int Value { get; }

        public Command(int id, string command, string value)
        {
            Id = id;
            Name = command;
            if (int.TryParse(value, out var intVal))
            {
                Value = intVal;
            }
        }

        public override string ToString() => $"{Name}({Value})";
        public void Execute(Turtle turtle)
        {
            switch (Id)
            {
                case LogoLexer.Forward:
                    var pos = turtle.Position;
                    var target = new Point(pos.X + MathF.Cos(turtle.Rotation) * Value, pos.Y - MathF.Sin(turtle.Rotation) * Value);
                    turtle.AddLine(pos, target);
                    turtle.Position = target;
                    break;
                case LogoLexer.Right:
                    turtle.Rotation -= Value * Turtle.ToRadians; 
                    break;
                default:
                    throw new Exception($"Unknown command: {Id}-{Name}");
            }
        }
    }
}
