using Logo2Svg.Turtle;

namespace LogoTests;

[TestClass]
public class Commands
{
    [TestMethod]
    [DataRow(0, "[ 99 0 0 ]")]
    [DataRow(1, @"""red")]
    [DataRow(2, @"""#ff0000")]
    [DataRow(3, @"""#FF0000")]
    public void Commands_SetPalette(int pos, string color)
    {
        TestUtils.ClearColourPalette();
        $"SetPalette {pos} {color}".Execute();
        var colour = ColourPalette.Palette[pos];
        Assert.IsNotNull(colour);
        colour.AssertColour(255, 0, 0);
    }
}