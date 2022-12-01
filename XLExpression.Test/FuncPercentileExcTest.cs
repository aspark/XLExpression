using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncPercentileExcTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("Percentile.Exc(F2:H3, 0.5)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(7);

            exp = ExpressionBuilder.Instance.Build("Percentile.Exc(F2:H3, 50%)");
            result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(7);

            exp = ExpressionBuilder.Instance.Build("Percentile.Exc(F2:H3, 60%)");
            result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(11.2);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("Percentile.Exc(F2:H3, E2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "E2", 0.5 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(7);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 0.6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(11.2);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 0.65 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(11.55);

            //result = exp.Invoke(new Dictionary<string, object> { { "E2", 0 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            //result.ShouldBe(null);

            //result = exp.Invoke(new Dictionary<string, object> { { "E2", 1 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            //result.ShouldBe(null);
        }
    }
}