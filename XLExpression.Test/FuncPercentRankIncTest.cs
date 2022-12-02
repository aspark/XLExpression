using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncPercentRankIncTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("PercentRank.Inc(F2:H3, 6)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.475);
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("PercentRank.Inc(F2:H3, E2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "E2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.475);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.6);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11.5 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.7);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 1 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 13 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(1);

            exp = ExpressionBuilder.Instance.Build("PercentRank.Inc(F2:H4, E2, 6)");
            result = exp.Invoke(new Dictionary<string, object> { { "E2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(0.671875);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(0.75);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11.5 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(0.8125);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 1 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(0);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", (object)null } });
            result.ShouldBe(0.714285);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11.5 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", "" } });
            result.ShouldBe(0.785714);
        }
    }
}