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

    [TestMethod]
    public void Parser_DefFunc_NoArgs()
    {
        var fakeVisitor = LogoFaker();
        @"to star
              repeat 5 [ fd 100 rt 144 ]
          end
          star".ToAst(fakeVisitor);

        A.CallTo(() => fakeVisitor.VisitDefineMethod(A<LogoParser.DefineMethodContext>.Ignored))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeVisitor.VisitRepeatStmt(A<LogoParser.RepeatStmtContext>.Ignored))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeVisitor.VisitSimpleCommand(A<LogoParser.SimpleCommandContext>.Ignored))
            .MustHaveHappenedTwiceExactly();
        A.CallTo(() => fakeVisitor.VisitCustomCommand(A<LogoParser.CustomCommandContext>.Ignored))
            .MustHaveHappenedOnceExactly();
    }
    
    [TestMethod]
    public void Parser_DefFunc_WithArgs()
    {
        var fakeVisitor = LogoFaker();
        @"to fern :size :sign
              if :size < 1 [ stop ]
              fd :size
              rt 70 * :sign fern :size * 0.5 :sign * -1 lt 70 * :sign
              fd :size
              lt 70 * :sign fern :size * 0.5 :sign rt 70 * :sign
              rt 7 * :sign fern :size - 1 :sign lt 7 * :sign
              bk :size * 2
          end
          fern 25 1".ToAst(fakeVisitor);

        A.CallTo(() => fakeVisitor.VisitDefineMethod(A<LogoParser.DefineMethodContext>.Ignored))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeVisitor.VisitIfStmt(A<LogoParser.IfStmtContext>.Ignored))
            .MustHaveHappenedOnceExactly();
        A.CallTo(() => fakeVisitor.VisitCustomCommand(A<LogoParser.CustomCommandContext>.Ignored))
            .MustHaveHappened(4, Times.Exactly);
    }
    
    private static LogoParserBaseVisitor<INode> LogoFaker()
        => A.Fake<LogoParserBaseVisitor<INode>>(options => options.CallsBaseMethods());
    
}
