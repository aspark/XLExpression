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
        public void Test2()
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

            exp = ExpressionBuilder.Instance.Build("Percentile.Exc(F2:H4, E2)");
            result = exp.Invoke(new Dictionary<string, object> { { "E2", 0.6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(3);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 0.65 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(7);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 0.75 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(11.5);

            //result = exp.Invoke(new Dictionary<string, object> { { "E2", 1 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G43", 3 }, { "H4", 3 } });
            //result.ShouldBe(1);

            //result = exp.Invoke(new Dictionary<string, object> { { "E2", 13 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G43", 3 }, { "H4", 3 } });
            //result.ShouldBe(13);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 0.75 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", (object)null } });
            result.ShouldBe(11.75);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 0.6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", "" } });
            result.ShouldBe(6.2);
        }
    }
}