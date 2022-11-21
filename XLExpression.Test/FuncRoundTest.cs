using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncRoundTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("Round(123.254, 2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(123.25);

            exp = ExpressionBuilder.Instance.Build("Round(123.254, 4)");
            result = exp.Invoke();
            result.ShouldBe(123.2540);

            exp = ExpressionBuilder.Instance.Build("Round(123.254, 3)");
            result = exp.Invoke();
            result.ShouldBe(123.254);

            exp = ExpressionBuilder.Instance.Build("Round(123.254, 1)");
            result = exp.Invoke();
            result.ShouldBe(123.3);

            exp = ExpressionBuilder.Instance.Build("Round(123.254, 0)");
            result = exp.Invoke();
            result.ShouldBe(123);

            exp = ExpressionBuilder.Instance.Build("Round(123.254, -1)");
            result = exp.Invoke();
            result.ShouldBe(120);

            exp = ExpressionBuilder.Instance.Build("Round(153.254, -2)");
            result = exp.Invoke();
            result.ShouldBe(200);
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("Round(A1, 2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { A1 = 153.562 });
            result.ShouldBe(153.56);

            result = exp.Invoke(new { A1 = 153.565 });
            result.ShouldBe(153.57);

            result = exp.Invoke(new { A1 = 153.56595 });
            result.ShouldBe(153.57);

            result = exp.Invoke(new { A1 = 153.5 });
            result.ShouldBe(153.50);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("ROUND(A1, A2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { A1 = 153.562, A2 = 2 });
            result.ShouldBe(153.56);

            result = exp.Invoke(new { A1 = 153.565, A2 = 3 });
            result.ShouldBe(153.565);

            result = exp.Invoke(new { A1 = 153.56595, A2 = 2 });
            result.ShouldBe(153.57);

            result = exp.Invoke(new { A1 = 153.5, A2 = -2 });
            result.ShouldBe(200);
        }

    }
}