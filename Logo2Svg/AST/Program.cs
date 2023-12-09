namespace Logo2Svg.AST
{
    public class Program : CommandBlock
    {
        public Program(IEnumerable<Command> cmdLst) : base(cmdLst)
        {
        }
        
        public override string ToString() => string.Join("\n", this);
    }
}