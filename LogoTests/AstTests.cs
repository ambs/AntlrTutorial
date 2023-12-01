using Logo2Svg.AST;
using Logo2Svg.Language;

namespace LogoTests;

[TestClass]
public class AstTests
{
    [TestMethod]
    [DataRow("forward", LogoLexer.Forward)]
    [DataRow("back", LogoLexer.Back)]
    [DataRow("fd", LogoLexer.Forward)]
    [DataRow("bk", LogoLexer.Back)]
    [DataRow("right", LogoLexer.Right)]
    [DataRow("rt", LogoLexer.Right)]
    [DataRow("left", LogoLexer.Left)]
    [DataRow("lt", LogoLexer.Left)]
    public void AstTests_BasicCommands(string cmd, int cmdId)
    {
        var tree = $"{cmd} 10".ToAst();

        var program = tree as Program;
        Assert.IsNotNull(program);
        Assert.AreEqual(1, program.Count);

        var command = program.First();
        Assert.IsNotNull(command);
        Assert.AreEqual(cmdId, command.Id);
        Assert.AreEqual(1, command.Params.Count);

        var param = command.Params.First() as ValueParam;
        Assert.IsNotNull(param);
        Assert.AreEqual(10f, param.FloatValue);
    }
}