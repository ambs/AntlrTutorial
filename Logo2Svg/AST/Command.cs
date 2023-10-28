using Logo2Svg.Language;

namespace Logo2Svg.AST
{
    public class Command : INode
    {
        public string Name { get; }
        public int Value { get; }

        public Command(string command, string value)
        {
            Name = command;
            if (int.TryParse(value, out var intVal))
            {
                Value = intVal;
            }
        }

        public override string ToString() => $"{Name}({Value})";
    }
}
