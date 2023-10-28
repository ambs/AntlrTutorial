
namespace Logo2Svg.AST
{
    public class Program : List<Command>, INode
    {
        public override string ToString() => string.Join("\n", this);
    }
}
