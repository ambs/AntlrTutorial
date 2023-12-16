using Antlr4.Runtime;
using Logo2Svg.AST;
using Logo2Svg.Language;

namespace LogoTests;

using FakeItEasy;

// dotnet add LogoTests package FakeItEasy

[TestClass]
public class Parser
{
    [TestMethod]
    public void Parser_BasicCommand()
    {
        var fakeVisitor = LogoFaker();
        "FD 10 RT 90 FD 20".ToAst(fakeVisitor);

        A.CallTo(() => fakeVisitor.VisitBasicCommand(A<LogoParser.BasicCommandContext>.Ignored))
            .MustHaveHappened(3, Times.Exactly);
    }

    [TestMethod]
    public void Parser_IfElse()
    {
        var fakeVisitor = LogoFaker();
        "IfElse [ 10 > 20 ] [ FD 10 ] [ BK 20 ]".ToAst(fakeVisitor);
        
        A.CallTo(() => fakeVisitor.VisitIfElseStmt(A<LogoParser.IfElseStmtContext>.Ignored))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeVisitor.VisitCmdBlock(A<LogoParser.CmdBlockContext>.Ignored)).MustHaveHappenedTwiceExactly();
    }

    private LogoParserBaseVisitor<INode> LogoFaker()
        => A.Fake<LogoParserBaseVisitor<INode>>(options => options.CallsBaseMethods());
    
}
