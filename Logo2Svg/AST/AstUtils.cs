namespace Logo2Svg.AST;

public static class AstUtils
{
    public static bool AsBool(this float v) => v is >= 0.5f or <= -0.5f;
    
    public static float AsFloat(this bool b) => b ? 1f : 0f;

}