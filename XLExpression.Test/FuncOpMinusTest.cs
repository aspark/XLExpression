using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncOpMinusTest
    {
        [Fact]
        public void MinusTest1()
        {
            var exp = ExpressionBuilder.Instance.Build("1-2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(-1);
        }

        [Fact]
        public void MinusTest2()
        {
            var exp = ExpressionBuilder.Instance.Build("3-2-1");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(0);
        }

        [Fact]
        public void MinusTest3()
        {
            var exp = ExpressionBuilder.Instance.Build("1-F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { F2 = 5 });

            result.ShouldBe(-4);
        }

        [Fact]
        public void MinusTest4()
        {
            var exp = ExpressionBuilder.Instance.Build("A1-F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { A1 = 50, F2 = 23 });

            result.ShouldBe(27);
        }

    }
}