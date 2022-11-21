using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncOpMulTest
    {
        [Fact]
        public void AddTest1()
        {
            var exp = ExpressionBuilder.Instance.Build("1*2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(2);
        }

        [Fact]
        public void AddTest2()
        {
            var exp = ExpressionBuilder.Instance.Build("2*3*4");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(24);
        }

        [Fact]
        public void AddTest3()
        {
            var exp = ExpressionBuilder.Instance.Build("2*F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { F2 = 8 });

            result.ShouldBe(16);
        }

        [Fact]
        public void AddTest4()
        {
            var exp = ExpressionBuilder.Instance.Build("A1*F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { A1 = 3, F2 = 8 });

            result.ShouldBe(24);
        }

    }
}