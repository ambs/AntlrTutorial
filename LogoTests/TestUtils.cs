using Antlr4.Runtime;
using Logo2Svg;
using Logo2Svg.AST;
using Logo2Svg.Language;
using Logo2Svg.SVG;

namespace LogoTests;

public static class TestUtils
{
    public static Parameter ToParameter(this string expr)
    {
        var input = new AntlrInputStream(expr);
        var lexer = new LogoLexer(input);
        var parser = new LogoParser(new CommonTokenStream(lexer));

        var exprContext = parser.expr();
        var visitor = new TreeVisitor();
        return visitor.Visit<Parameter>(exprContext);
    }

    public static INode ToAst(this string code) => code.ToAst(new TreeVisitor());
    
    public static INode ToAst(this string code, LogoParserBaseVisitor<INode> visitor)
    {
        var input = new AntlrInputStream(code);
        var lexer = new LogoLexer(input);
        var parser = new LogoParser(new CommonTokenStream(lexer));

        var tree = parser.program();
        return visitor.Visit(tree);
    }

    public static (Turtle, Program?) Execute(this string code)
    {
        var tree = code.ToAst();
        var turtle = new Turtle();
        tree.Execute(turtle);
        return (turtle, tree as Program);
    }

    public static void AssertColour(this Colour colour, int red, int green, int blue)
    {
        Assert.AreEqual(colour.Red, red);
        Assert.AreEqual(colour.Green, green);
        Assert.AreEqual(colour.Blue, blue);
    }
}