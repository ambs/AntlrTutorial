using Antlr4.Runtime;
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
    
}