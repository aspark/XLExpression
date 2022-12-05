using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncSumIfTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("SUMIF(F2:H2, 2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 } });

            result.ShouldBe(2);
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("SUMIF(F2:H3, \">3\")");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 11 }, { "G3", 12 }, { "H3", 13 } });
            result.ShouldBe(36);

            exp = ExpressionBuilder.Instance.Build("SUMIF(2:2, \">3\")");
            result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 11 }, { "G3", 12 }, { "H3", 13 } });
            result.ShouldBe(0);

            exp = ExpressionBuilder.Instance.Build("SUMIF(2:3, \">3\")");
            result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 11 }, { "G3", 12 }, { "H3", 13 } });
            result.ShouldBe(36);

            exp = ExpressionBuilder.Instance.Build("SUMIF(F:F, \">=3\")");
            result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 11 }, { "G3", 12 }, { "H3", 13 } });
            result.ShouldBe(11);

            exp = ExpressionBuilder.Instance.Build("SUMIF(F:F, 11, G:G)");
            result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "F3", 11 }, { "G3", 12 }, { "F4", 20 }, { "G4", 13 } });
            result.ShouldBe(12);

            exp = ExpressionBuilder.Instance.Build("SUMIF(F:F, \">3\", G:G)");
            result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "F3", 11 }, { "G3", 12 }, { "F4", 20 }, { "G4", 13 } });
            result.ShouldBe(25);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("SUMIF(F2:H3, A2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "A2", 1 }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 11 }, { "G3", 12 }, { "H3", 13 } });
            result.ShouldBe(1);

            result = exp.Invoke(new Dictionary<string, object> { { "A2", ">3" }, { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 11 }, { "G3", 12 }, { "H3", 13 } });
            result.ShouldBe(36);
        }

    }
}