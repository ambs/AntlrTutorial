namespace Logo2Svg.AST;

public class CommandBlock :  List<Command>, INode
{
    public CommandBlock(IEnumerable<Command> cmdLst) : base(cmdLst)
    { }

    public void Execute(Turtle turtle)
    {
        foreach (var cmd in this)
        {
            cmd.Execute(turtle);
            if (turtle.IsExiting) break;
        }
    }
        
    public override string ToString() => $"[{string.Join("\n", this)}]";
}