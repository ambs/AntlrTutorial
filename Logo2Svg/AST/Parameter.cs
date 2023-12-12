namespace Logo2Svg.AST;

/// <summary>
/// Abstract class for an expression parameter.
/// </summary>
public abstract class Parameter : INode
{
    /// <summary>
    /// The evaluation for an expression parameter.
    /// </summary>
    /// <param name="turtle">The turtle information.</param>
    /// <returns>A float value after evaluating the parameter.</returns>
    public abstract float Value(Turtle turtle);
    
    /// <summary>
    /// Parameters can't be executed, but rather evaluated. 
    /// </summary>
    /// <param name="turtle">The turtle information.</param>
    /// <exception cref="NotImplementedException">This method can't be implemented.</exception>
    public void Execute(Turtle turtle) => throw new NotImplementedException();
}
