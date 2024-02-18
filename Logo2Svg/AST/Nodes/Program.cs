namespace Logo2Svg.AST.Nodes;

/// <summary>
/// Represents the top level of the LOGO AST.
/// </summary>
public class Program : CommandBlock
{
    /// <summary>
    /// Constructor, given a list of commands.
    /// </summary>
    /// <param name="cmdLst">The list of commands to add to the program.</param>
    public Program(IEnumerable<Command> cmdLst) : base(cmdLst) { }
    
    /// <summary>
    /// Stringification of a program.
    /// </summary>
    /// <returns>The stringification of the program.</returns>
    public override string ToString() => string.Join("\n", this);
}
