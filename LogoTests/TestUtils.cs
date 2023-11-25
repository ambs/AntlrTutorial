using Antlr4.Runtime;
using Logo2Svg.AST;
using Logo2Svg.Language;

namespace LogoTests;

public static class TestUtils
{
    public static Parameter ToParameter(this string expr)
    {
        AntlrInputStream input = new AntlrInputStream(expr);
        var lexer = new LogoLexer(input);
        var parser = new LogoParser(new CommonTokenStream(lexer));

        var exprContext = parser.expr();
        var visitor = new TreeVisitor();
        return visitor.Visit<Parameter>(exprContext);
    }
}