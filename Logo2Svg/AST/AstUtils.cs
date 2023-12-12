namespace Logo2Svg.AST;

/// <summary>
/// Extension methods useful for the Abstract Syntax Tree management. 
/// </summary>
public static class AstUtils
{
    /// <summary>
    /// Converts a float value to boolean. A float value is false if inside the interval ]-0.5, 0.5[, and true otherwise.
    /// </summary>
    /// <param name="value">The float value to be converted.</param>
    /// <returns>The respective boolean value.</returns>
    public static bool AsBool(this float value) => value is >= 0.5f or <= -0.5f;
    
    /// <summary>
    /// Converts a boolean value to float. 0f if false, 1f if true.
    /// </summary>
    /// <param name="value">The boolean value to be converted.</param>
    /// <returns>The respective float value.</returns>
    public static float AsFloat(this bool value) => value ? 1f : 0f;

}