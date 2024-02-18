using Logo2Svg.AST;
using Logo2Svg.AST.Nodes;
using Logo2Svg.Language;

namespace LogoTests;

[TestClass]
public class Expressions
{
    [TestMethod]
    public void SimpleValue()
    {
        const float value = 10.4f;
        Parameter param = new ValueParam(value);
        
        Assert.AreEqual(value, param.Value(null), "Value correctly stored");
    }

    [TestMethod]
    public void SumTwoValues()
    {
        const float a = 10.4f, b = 30.2f;
        ValueParam pA = new(a), pB = new(b);
        Parameter sum = new Expression(LogoLexer.Sum, pA, pB);
        
        Assert.AreEqual(a + b, sum.Value(null), "Sum correctly computed");
    }

    [TestMethod]
    public void SumMultipleValues()
    {
        var r = new Random(666);
        var values = Enumerable.Range(0, 10).Select(_ => r.NextSingle()).ToArray();
        var parameters = values.Select<float, Parameter>(v => new ValueParam(v)).ToArray();
        Parameter sumExpr = new Expression(LogoLexer.Sum, parameters);
        var sumValue = values.Aggregate(0f, (a, b) => a + b);

        Assert.AreEqual(sumValue, sumExpr.Value(null), "Sum correctly computed");
    }
}