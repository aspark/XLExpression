using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncOpPowTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("2^3");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();
            result.ShouldBe(8);
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("2^2^2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();
            result.ShouldBe(16);

            exp = ExpressionBuilder.Instance.Build("(2^2+1)^2");

            result = exp.Invoke();
            result.ShouldBe(25);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("2^F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { F2 = 4 });

            result.ShouldBe(16);
        }

        [Fact]
        public void Test4()
        {
            var exp = ExpressionBuilder.Instance.Build("A1^F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { A1 = 3, F2 = 4 });

            result.ShouldBe(81);
        }

    }
}