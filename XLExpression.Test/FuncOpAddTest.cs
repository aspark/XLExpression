using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncOpAddTest
    {
        //ADD
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("1+2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(3);
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("1+2+3");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(6);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("1+F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { F2 = "abc" });

            result.ShouldBe("1abc");
        }

        [Fact]
        public void Test4()
        {
            var exp = ExpressionBuilder.Instance.Build("A1+F2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { A1 = "ABC", F2 = "abc" });

            result.ShouldBe("ABCabc");
        }

    }
}