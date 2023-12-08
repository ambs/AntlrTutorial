namespace Logo2Svg.AST;

public static class AstUtils
{
    public static bool AsBool(this float v) => v is >= 1 or <= -1;
    
    public static float AsFloat(this bool b) => b ? 1f : 0f;

}