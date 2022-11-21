using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncSumTest
    {
        //SUM
        [Fact]
        public void SumTest1()
        {
            var exp = ExpressionBuilder.Instance.Build("SUM(F2:H2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 } });

            result.ShouldBe(6);
        }

        [Fact]
        public void SumTest2()
        {
            var exp = ExpressionBuilder.Instance.Build("SUM(F2:H3)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 11 }, { "G3", 12 }, { "H3", 13 } });

            result.ShouldBe(42);
        }

        [Fact]
        public void SumTest3()
        {
            var exp = ExpressionBuilder.Instance.Build("SUM(F2:H2, F3:H3)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 11 }, { "G3", 12 }, { "H3", 13 } });

            result.ShouldBe(42);
        }

    }
}