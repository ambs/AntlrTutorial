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

    [TestMethod]
    public void VariableThings()
    {
        var tree = @"MAKE ""a 10 MAKE ""b 20 MAKE ""c thing ""a + :b".ToAst() as Program;
        Assert.IsNotNull(tree);
        Assert.AreEqual(3, tree.Count);

        var names = new[] {"a", "b", "c"};
        for (var i = 0; i < 3; i++)
        {
            Assert.AreEqual(LogoLexer.Make, tree[i].Id);
            Assert.AreEqual(names[i], (tree[i].Params[0] as VarName)?.Name);
        }
        Assert.AreEqual(10f, tree[0].Parameter<ValueParam>(1).FloatValue);
        Assert.AreEqual(20f, tree[1].Parameter<ValueParam>(1).FloatValue);
        var sum = tree[2].Parameter<ExprParam>(1);
        Assert.IsNotNull(sum);
        Assert.AreEqual(LogoLexer.Sum, sum.Op);
        for (var i = 0; i < 2; i++)
        {
            var varName = sum.Parameters[i] as VarName;
            Assert.IsNotNull(varName);
            Assert.AreEqual(names[i], varName.Name);
        }
    }
}
