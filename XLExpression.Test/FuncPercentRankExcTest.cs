using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncPercentRankExcTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("PercentRank.Exc(F2:H3, 6, 6)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.482142);

            exp = ExpressionBuilder.Instance.Build("PercentRank.Exc(F2:H3, 6)");
            result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.482);
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("PercentRank.Exc(F2:H3, E2, D2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "E2", 6 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.482142);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.571428);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11.5 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.642857);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 1 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.142857);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 13 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 } });
            result.ShouldBe(0.857142);

            exp = ExpressionBuilder.Instance.Build("PercentRank.Exc(F2:H4, E2, 6)");
            result = exp.Invoke(new Dictionary<string, object> { { "E2", 6 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(0.6375);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(0.7);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11.5 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(0.75);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 1 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", 3 } });
            result.ShouldBe(0.1);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", (object)null } });
            result.ShouldBe(0.666666);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", 11.5 }, { "D2", 6 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 13 }, { "G3", 12 }, { "H3", 11 }, { "F4", 3 }, { "G4", 3 }, { "H4", "" } });
            result.ShouldBe(0.722222);
        }
    }
}