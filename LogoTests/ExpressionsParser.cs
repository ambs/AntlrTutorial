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
    public void ParseValue2(string expr, float value)
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
    [DataRow("4 + 3.2", 4+3.2f)]
    [DataRow("3 - 2.1", 3-2.1f)]
    [DataRow("3 * 2.3", 3 * 2.3f)]
    [DataRow("2 / 4", 0.5f)]
    [DataRow("9.3 ^ 2", 9.3f*9.3f)]
    [DataRow("(sum 1 2 3 4 5 6 7 8 9)", 45f)]
    [DataRow("(product 4 3 2 1)", 24f)]
    [DataRow("(quotient 4)", 1/4f)]
    public void ParseBasicOperators(string expr, float value)
    {
        var param = expr.ToParameter();
        var result = param.Value(null);
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
    
}