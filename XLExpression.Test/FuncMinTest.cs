using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncMinTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("MIN(1,2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(1);
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("MIN(1, 2, 3, 4)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(1);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("MIN(3, 4, F2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { F2 = 6 });

            result.ShouldBe(3);
        }

        [Fact]
        public void Test4()
        {
            var exp = ExpressionBuilder.Instance.Build("MIN(3, A1, B2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { A1 = 1, B2 = 3 });

            result.ShouldBe(1);

            result = exp.Invoke(new { A1 = 3, B2 = -1 });

            result.ShouldBe(-1);

            exp = ExpressionBuilder.Instance.Build("MIN(A1, B2)");
            result = exp.Invoke(new { A1 = "a", B2 = "b" });
            result.ShouldBe(0);

            result = exp.Invoke(new { A1 = "a", B2 = "" });
            result.ShouldBe(0);

            exp = ExpressionBuilder.Instance.Build("MIN(3, A1, B2)");
            result = exp.Invoke(new { A1 = "a", B2 = "b" });
            result.ShouldBe(3);

            result = exp.Invoke(new { A1 = "a", B2 = "" });
            result.ShouldBe(3);

            exp = ExpressionBuilder.Instance.Build("MIN(-1, A1, B2)");
            result = exp.Invoke(new { A1 = "a", B2 = "b" });
            result.ShouldBe(-1);

            exp = ExpressionBuilder.Instance.Build("MIN(A1, -1, B2)");
            result = exp.Invoke(new { A1 = "a", B2 = "b" });
            result.ShouldBe(-1);
        }

    }
}