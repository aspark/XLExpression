using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncOpDivTest
    {
        [Fact]
        public void DivTest1()
        {
            var exp = ExpressionBuilder.Instance.Build("1/2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(0.5);
        }

        [Fact]
        public void DivTest2()
        {
            var exp = ExpressionBuilder.Instance.Build("1/2/4");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(0.125);
        }

        [Fact]
        public void DivTest3()
        {
            var exp = ExpressionBuilder.Instance.Build("1/F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { F2 = 4 });

            result.ShouldBe(0.25);
        }

        [Fact]
        public void DivTest4()
        {
            var exp = ExpressionBuilder.Instance.Build("A1/F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { A1 = 2, F2 = 8 });

            result.ShouldBe(0.25);
        }

    }
}