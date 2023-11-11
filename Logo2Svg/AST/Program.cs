namespace Logo2Svg.AST
{
    public class Program : List<Command>, INode
    {
        public void Execute(Turtle turtle)
        {
            ForEach(cmd => cmd.Execute(turtle));
        }

        public override string ToString() => string.Join("\n", this);
    }
}