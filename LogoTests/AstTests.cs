using Logo2Svg.AST;
using Logo2Svg.AST.Nodes;
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
        var sum = tree[2].Parameter<Expression>(1);
        Assert.IsNotNull(sum);
        Assert.AreEqual(LogoLexer.Sum, sum.Op);
        for (var i = 0; i < 2; i++)
        {
            var varName = sum.Parameters[i] as VarName;
            Assert.IsNotNull(varName);
            Assert.AreEqual(names[i], varName.Name);
        }
    }

    [TestMethod]
    public void IfElse()
    {
        var tree = @"IfElse [ :b < 69 ] [ MAKE ""d 0 ] [ MAKE ""d 1 ]".ToAst() as Program;
        Assert.IsNotNull(tree);
        Assert.AreEqual(1, tree.Count);
        var cmd = tree[0];
        Assert.IsNotNull(cmd);
        Assert.AreEqual(LogoLexer.IfElse, cmd.Id);
        Assert.AreEqual(3, cmd.Params.Count);
        var cmp = cmd.Parameter<Expression>(0);
        Assert.IsNotNull(cmd);
        Assert.AreEqual(LogoLexer.Less, cmp.Op);
        var trueBranch = cmd.Parameter<CommandBlock>(1);
        Assert.AreEqual(1, trueBranch.Count);
        Assert.AreEqual(LogoLexer.Make, trueBranch[0].Id);
        var falseBranch = cmd.Parameter<CommandBlock>(2);
        Assert.AreEqual(1, falseBranch.Count);
        Assert.AreEqual(LogoLexer.Make, falseBranch[0].Id);
    }

    [TestMethod]
    public void ColorPalette_CSSColor()
    {
        var tree = @"SetPalette 1 ""#ff00ff".ToAst() as Program;
        Assert.IsNotNull(tree);
        Assert.AreEqual(1, tree.Count);
        var cmd = tree[0];
        Assert.IsNotNull(cmd);
        Assert.AreEqual(LogoLexer.SetPalette, cmd.Id);
        Assert.AreEqual(2, cmd.Params.Count);
        var position = cmd.Parameter<ValueParam>(0);
        Assert.IsNotNull(position);
        var colour = cmd.Parameter<ColourNode>(1);
        Assert.IsNotNull(colour);
    }
    
    [TestMethod]
    public void ColorPalette_LogoColor()
    {
        var tree = @"SetPalette 1 [1 2 3]".ToAst() as Program;
        Assert.IsNotNull(tree);
        Assert.AreEqual(1, tree.Count);
        var cmd = tree[0];
        Assert.IsNotNull(cmd);
        Assert.AreEqual(LogoLexer.SetPalette, cmd.Id);
        Assert.AreEqual(2, cmd.Params.Count);
        var position = cmd.Parameter<ValueParam>(0);
        Assert.IsNotNull(position);
        var colour = cmd.Parameter<ColourNode>(1);
        Assert.IsNotNull(colour);
    }

    [TestMethod]
    public void MethodDefinition_NoArgs()
    {
        var tree = @"to star
                        repeat 5 [ fd 100 rt 144 ]
                      end
                      star".ToAst() as Program;
        Assert.IsNotNull(tree);
        Assert.AreEqual(2, tree.Count);

        var toCommand = tree.First();
        Assert.AreEqual(LogoLexer.To, toCommand.Id);
        var methodDef = toCommand.Parameter<Method>(0);
        Assert.IsNotNull(methodDef);
        Assert.AreEqual("star/0", methodDef.Name);
        var methodCode = methodDef.Code;
        Assert.IsNotNull(methodCode);
        Assert.AreEqual(1, methodCode.Count);
        Assert.AreEqual(LogoLexer.Repeat, methodCode.First().Id);

        var invocation = tree.Last();
        Assert.AreEqual(-1, invocation.Id);
        Assert.AreEqual("star", invocation.Name);
        Assert.AreEqual(0, invocation.Params.Count);
    }
}