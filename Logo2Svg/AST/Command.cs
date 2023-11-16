using Logo2Svg.Language;
using Logo2Svg.SVG;

namespace Logo2Svg.AST
{
    public class Command : INode
    {
        public string Name { get; }
        public int Id { get;  }
        public List<IParameter> Params;

        public Command(int id, string command, params IParameter[] @params)
        {
            Id = id;
            Name = command;
            Params = @params is null ? null : new List<IParameter>(@params);
        }

        public override string ToString()
        {
            var parameters = Params is null ? "" : string.Join(",", Params); 
            return $"{Name}({parameters})";
        }

        private T Parameter<T>(int i)
        {
            if (i < 0 || i >= Params.Count) throw new IndexOutOfRangeException("Parameter out of bounds");
            if (Params[i] is not T p) throw new InvalidCastException("Invalid parameter type");
            return p;
        }

        public void Execute(Turtle turtle)
        {
            switch (Id)
            {
                case LogoLexer.Forward:
                {
                    var value = Parameter<ValueParam>(0).Value;
                    var pos = turtle.Position;
                    var target = new Point(pos.X + MathF.Cos(turtle.Rotation) * value,
                        pos.Y - MathF.Sin(turtle.Rotation) * value);
                    turtle.AddLine(pos, target);
                    turtle.Position = target;
                    break;
                }
                case LogoLexer.Back:
                {
                    var value = Parameter<ValueParam>(0).Value;
                    var pos = turtle.Position;
                    var target = new Point(pos.X + MathF.Cos(turtle.Rotation) * value,
                        pos.Y + MathF.Sin(turtle.Rotation) * value);
                    turtle.AddLine(pos, target);
                    turtle.Position = target;
                    break;
                }
                case LogoLexer.Right:
                {
                    var value = Parameter<ValueParam>(0).Value;
                    turtle.Rotation -= value * Turtle.ToRadians;
                    break;
                }
                case LogoLexer.Left:
                {
                    var value = Parameter<ValueParam>(0).Value;
                    turtle.Rotation += value * Turtle.ToRadians;
                    break;
                }
                
                case LogoLexer.Home:
                    turtle.Reset();
                    break;
                case LogoLexer.SetXY:
                case LogoLexer.SetPos:
                    turtle.Position = Parameter<PointParam>(0).Point;
                    break;

                case LogoLexer.SetX:
                    turtle.Position.X = Parameter<ValueParam>(0).Value;
                    break;
                case LogoLexer.SetY:
                    turtle.Position.Y = Parameter<ValueParam>(0).Value;
                    break;
                case LogoLexer.SetH:
                    turtle.Rotation = Parameter<ValueParam>(0).Value;
                    break;

                case LogoLexer.Arc:
                {
                    var angle = Parameter<ValueParam>(0).Value * Turtle.ToRadians;
                    var radius = Parameter<ValueParam>(0).Value;
                    turtle.AddArc(turtle.Position, turtle.Rotation, radius, angle);
                    break;
                }

                
                default:
                    throw new Exception($"Unknown command: {Id}-{Name}");
            }
        }
    }
}
