using Logo2Svg.Language;
using Logo2Svg.SVG;

namespace Logo2Svg.AST
{
    public class Command : INode
    {
        private string Name { get; }
        public int Id { get;  }
        public List<Parameter> Params;

        public Command(int id, string command, params Parameter[] @params)
        {
            Id = id;
            Name = command;
            Params = @params is null ? null : new List<Parameter>(@params);
        }

        public override string ToString()
        {
            var parameters = Params is null ? "" : string.Join(",", Params); 
            return $"{Name}({parameters})";
        }

        private Parameter Parameter(int i) => Params[i];

        public T Parameter<T>(int i)
        {
            if (i < 0 || i >= Params.Count) throw new IndexOutOfRangeException("Parameter out of bounds");
            if (Params[i] is not T p) throw new InvalidCastException("Invalid parameter type");
            return p;
        }

        public void Execute(Turtle turtle)
        {
            switch (Id)
            {
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
}
