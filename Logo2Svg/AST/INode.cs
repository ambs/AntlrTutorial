namespace Logo2Svg.AST;

/// <summary>
/// Interface for the AST nodes.
/// </summary>
public interface INode
{
    /// <summary>
    /// Executes the AST node.
    /// </summary>
    /// <param name="turtle">The turtle information.</param>
    public void Execute(Turtle turtle);
}
