using Logo2Svg.Turtle;

namespace Logo2Svg.AST;

/// <summary>
/// A parameter representing a variable name.
/// </summary>
public class VarName : Parameter
{
    /// <summary>
    /// The name of the variable.
    /// </summary>
    public readonly string Name;

    /// <summary>
    /// Constructor of the variable parameter.
    /// </summary>
    /// <param name="varName">The variable name.</param>
    public VarName(string varName) => Name = varName;
    
    /// <summary>
    /// Evaluator for a variable. Queries the symbol table and returns its value.
    /// </summary>
    /// <param name="turtleState">The turtle data, that includes the symbol table.</param>
    /// <returns>The stored value, or 0 otherwise.</returns>
    public override float Value(TurtleState turtleState) => turtleState.RetrieveVariable(Name, out var expr) ? expr : 0f;

    /// <summary>
    /// Stringification of the variable name.
    /// </summary>
    /// <returns>The string representation of the variable.</returns>
    public override string ToString() => $@"""{Name}";
}