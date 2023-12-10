namespace Logo2Svg.AST;

/// <summary>
/// Represents a block of commands used in compound statements.
/// </summary>
public class CommandBlock :  List<Command>, INode
{
    /// <summary>
    /// Basic constructor given a list of commands.
    /// </summary>
    /// <param name="cmdLst">The list of commands to add to the block.</param>
    public CommandBlock(IEnumerable<Command> cmdLst) : base(cmdLst) { }

    /// <summary>
    /// Executes the current block of commands.
    /// </summary>
    /// <param name="turtle">Turtle information.</param>
    public void Execute(Turtle turtle)
    {
        foreach (var cmd in this)
        {
            cmd.Execute(turtle);
            if (turtle.IsExiting) break;
        }
    }
        
    /// <summary>
    /// String representation of the block of commands.
    /// </summary>
    /// <returns>The stringification of the block.</returns>
    public override string ToString() => $"[{string.Join("\n", this)}]";
}