using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncOpGreaterThanTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("1>2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(false);
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("2>1");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(true);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("1>F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { F2 = -1 });

            result.ShouldBe(true);
        }

        [Fact]
        public void Test4()
        {
            var exp = ExpressionBuilder.Instance.Build("A1>B2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { A1 = 3, B2 = 3 });
            result.ShouldBe(false);

            result = exp.Invoke(new { A1 = 3, B2 = 1 });
            result.ShouldBe(true);

            result = exp.Invoke(new { A1 = new DateTime(2022, 11, 21), B2 = "2022-11-22" });
            result.ShouldBe(false);

            result = exp.Invoke(new { A1 = new DateTime(2022, 11, 21), B2 = "2022-11-20" });
            result.ShouldBe(true);
        }

    }
}