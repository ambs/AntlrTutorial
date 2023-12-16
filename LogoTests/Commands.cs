using Logo2Svg.AST;
using Logo2Svg.SVG;

namespace LogoTests;

[TestClass]
public class Commands
{
    [TestMethod]
    [DataRow("[ 99 0 0 ]")]
    [DataRow(@"""red")]
    [DataRow(@"""#ff0000")]
    [DataRow(@"""#FF0000")]
    public void Commands_SetPalette(string c)
    {
        $"SetPalette 0 {c}".Execute();
        var colour = Colour.Palette[0];
        Assert.IsNotNull(colour);
        colour.AssertColour(255, 0, 0);
    }
}