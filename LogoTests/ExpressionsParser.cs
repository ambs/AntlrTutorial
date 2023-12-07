using Antlr4.Runtime;
using Logo2Svg;
using Logo2Svg.AST;
using Logo2Svg.Language;

namespace LogoTests;

[TestClass]
public class ExpressionsParser
{
    [TestMethod]
    [DataRow("4", 4f)]
    [DataRow("4.5", 4.5f)]
    public void ParseValue(string expr, float value)
    {
        var param = expr.ToParameter();
        var result = param.Value(null);
        Assert.AreEqual(value, result, $"Correctly parse of {expr}");
    }
    
    [TestMethod]
    [DataRow("sum 4 3.2", 4+3.2f)]
    [DataRow("difference 4 3.2", 4-3.2f)]
    [DataRow("product 2 4.3", 2 * 4.3f)]
    [DataRow("quotient 3 2", 3f/2f)]
    [DataRow("power 2 3", 2*2*2f)]
    [DataRow("remainder 2 2", 0f)]
    [DataRow("modulo 2 2", 0f)]
    [DataRow("remainder -3 2", -1f)]
    [DataRow("remainder 3 -2", 1f)]
    [DataRow("modulo -3 2", 1f)]
    [DataRow("modulo 3 -2", -1f)]
    [DataRow("minus 4", -4f)]
    [DataRow("-3.2", -3.2f)]
    [DataRow("4 + 3.2", 4+3.2f)]
    [DataRow("3 - 2.1", 3-2.1f)]
    [DataRow("3 * 2.3", 3 * 2.3f)]
    [DataRow("2 / 4", 0.5f)]
    [DataRow("2 % 4", 2f)]
    [DataRow("9.3 ^ 2", 9.3f*9.3f)]
    [DataRow("(sum 1 2 3 4 5 6 7 8 9)", 45f)]
    [DataRow("(product 4 3 2 1)", 24f)]
    [DataRow("(quotient 4)", 1/4f)]
    [DataRow("abs -10.2", 10.2f)]
    [DataRow("int 5.5", 5f)]
    [DataRow("round 5.5", 6f)]
    [DataRow("sqrt 4", 2f)]
    [DataRow("sqrt 5", 2.236f, 0.001f)]
    [DataRow("exp 4", 54.598f, 0.001f)]
    [DataRow("Log10 11", 1.041f, 0.001f)]
    [DataRow("Ln 10", 2.302f, 0.001f)]
    [DataRow("sin 3.1415", 0f, 0.001f)]
    [DataRow("cos 3.1415", -1f, 0.001f)]
    [DataRow("tan 3.1415", 0f, 0.001f)]
    [DataRow("radsin 180", 0f, 0.001f)]
    [DataRow("radcos 180", -1f, 0.001f)]
    [DataRow("radtan 180", 0f, 0.001f)]
    [DataRow("arctan 3.1415", 1.2626f, 0.001f)]
    [DataRow("radarctan 180", 1.2626f, 0.001f)]
    [DataRow("(arctan 3.1415 3.1415)", MathF.PI / 4f, 0.001f)]
    [DataRow("(radarctan 180 180)", MathF.PI / 4f, 0.001f)]
    
    [DataRow("1 < 2", 1f)]
    [DataRow("2 < 1", 0f)]
    [DataRow("lessp 1 2", 1f)]
    [DataRow("less? 2 1", 0f)]
    [DataRow("1 > 2", 0f)]
    [DataRow("2 > 1", 1f)]
    [DataRow("greaterp 1 2", 0f)]
    [DataRow("greater? 2 1", 1f)]
    [DataRow("1 >= 2", 0f)]
    [DataRow("2 >= 1", 1f)]
    [DataRow("2 >= 2", 1f)]
    [DataRow("greaterequalp 1 2", 0f)]
    [DataRow("greaterequal? 2 1", 1f)]
    [DataRow("greaterequalp 2 2", 1f)]
    [DataRow("1 <= 2", 1f)]
    [DataRow("2 <= 1", 0f)]
    [DataRow("2 <= 2", 1f)]
    [DataRow("lessequalp 1 2", 1f)]
    [DataRow("lessequal? 2 1", 0f)]
    [DataRow("lessequalp 2 2", 1f)]
    public void ParseBasicOperators(string expr, float value, float? delta = null)
    {
        var param = expr.ToParameter();
        var result = param.Value(null);
        if (delta.HasValue)
            Assert.AreEqual(value, result, delta.Value, $"Correctly parse of {expr}");
        else
            Assert.AreEqual(value, result, $"Correctly parse of {expr}");
    }

    
    [TestMethod]
    [DataRow("2 * 3 + 4", 10f)]
    [DataRow("4 + 3 * 2", 10f)]
    [DataRow("2 + 3 ^ 2", 11f)]
    [DataRow("3 ^ 2 + 2", 11f)]
    [DataRow("3 ^ 2 ^ 3", 6561f)]
    [DataRow(" 3 * - 2", -6f)]
    
    [DataRow("2 + 2 >= 4", 1f)]
    [DataRow("2 + 2 <= 2", 0f)]
    public void Priorities(string expr, float value)
    {
        var param = expr.ToParameter();
        var result = param.Value(null);
        Assert.AreEqual(value, result, $"Correctly priorities for {expr}");
    }

    [TestMethod]
    public void DivisionByZero()
    {
        Assert.ThrowsException<DivideByZeroException>(() => "10/0".ToParameter().Value(null));
    }
    
    [TestMethod]
    [DataRow("sum 4 3.2", "(sum (4) (3.2))")]
    [DataRow("difference 4 3.2", "(difference (4) (3.2))")]
    [DataRow("product 2 4.3", "(product (2) (4.3))")]
    [DataRow("quotient 3 2", "(quotient (3) (2))")]
    [DataRow("power 2 3", "(power (2) (3))")]
    [DataRow("remainder 2 2", "(remainder (2) (2))")]
    [DataRow("modulo 2 2", "(modulo (2) (2))")]
    [DataRow("remainder -3 2", "(remainder ((minus (3))) (2))")]
    [DataRow("remainder 3 -2", "(remainder (3) ((minus (2))))")]
    [DataRow("modulo -3 2", "(modulo ((minus (3))) (2))")]
    [DataRow("modulo 3 -2", "(modulo (3) ((minus (2))))")]
    [DataRow("minus 4", "(minus (4))")]
    [DataRow("-3.2", "(minus (3.2))")]
    [DataRow("4 + 3.2", "(sum (4) (3.2))")]
    [DataRow("3 - 2.1", "(difference (3) (2.1))")]
    [DataRow("3 * 2.3", "(product (3) (2.3))")]
    [DataRow("2 / 4", "(quotient (2) (4))")]
    [DataRow("2 % 4", "(remainder (2) (4))")]
    [DataRow("9.3 ^ 2", "(power (9.3) (2))")]
    [DataRow("(sum 1 2 3 4 5 6 7 8 9)", "(sum (1) (2) (3) (4) (5) (6) (7) (8) (9))")]
    [DataRow("(product 4 3 2 1)", "(product (4) (3) (2) (1))")]
    [DataRow("(quotient 4)", "(quotient (4))")]
    [DataRow("abs -10.2", "(abs ((minus (10.2))))")]
    [DataRow("int 5.5", "(int (5.5))")]
    [DataRow("round 5.5", "(round (5.5))")]
    [DataRow("(sqrt 4)", "(sqrt (4))")]
    [DataRow("sqrt 5", "(sqrt (5))")]
    [DataRow("exp 4", "(exp (4))")]
    [DataRow("Log10 11", "(log10 (11))")]
    [DataRow("Ln 10", "(ln (10))")]
    [DataRow("sin 3.1415", "(sin (3.1415))")]
    [DataRow("cos 3.1415", "(cos (3.1415))")]
    [DataRow("tan 3.1415", "(tan (3.1415))")]
    [DataRow("radsin 180", "(radsin (180))")]
    [DataRow("(radcos 180)", "(radcos (180))")]
    [DataRow("radtan 180", "(radtan (180))")]
    [DataRow("arctan 3.1415", "(arctan (3.1415))")]
    [DataRow("radarctan 180", "(radarctan (180))")]
    [DataRow("(arctan 3.1415 3.1415)", "(arctan (3.1415) (3.1415))")]
    [DataRow("(radarctan 180 180)", "(radarctan (180) (180))")]
    [DataRow("2 * 3 + 4", "(sum ((product (2) (3))) (4))")]
    [DataRow("4 + 3 * 2", "(sum (4) ((product (3) (2))))")]
    [DataRow("2 + 3 ^ 2", "(sum (2) ((power (3) (2))))")]
    [DataRow("3 ^ 2 + 2", "(sum ((power (3) (2))) (2))")]
    [DataRow("3 ^ 2 ^ 3", "(power (3) ((power (2) (3))))")]
    [DataRow(" 3 * - 2", "(product (3) ((minus (2))))")]
    public void Stringification(string expr, string expected)
    {
        var param = expr.ToParameter();
        var result = param.ToString();
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void Variable()
    {
        var tree = @"MAKE ""a 10 MAKE ""b 20 MAKE ""c thing ""a + :b".ToAst();
        var turtle = new Turtle();
        tree.Execute(turtle);
        Assert.IsTrue(turtle.RetrieveVariable("a", out var val1));
        Assert.AreEqual(10, val1);
        Assert.IsTrue(turtle.RetrieveVariable("b", out var val2));
        Assert.AreEqual(20, val2);
        Assert.IsTrue(turtle.RetrieveVariable("c", out var val3));
        Assert.AreEqual(30, val3);
    }
}