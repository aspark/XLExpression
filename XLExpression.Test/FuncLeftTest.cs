using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncLeftTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("Left(\"abc\", 1)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();
            result.ShouldBe("a");

            exp = ExpressionBuilder.Instance.Build("Left(\"abc\", 2)");
            result = exp.Invoke();
            result.ShouldBe("ab");

            exp = ExpressionBuilder.Instance.Build("Left(\"abc\")");
            result = exp.Invoke();
            result.ShouldBe("a");
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("Left(F2,2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" } });
            result.ShouldBe("ab");
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("Left(F2,G2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", 2 } });
            result.ShouldBe("ab");

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" } });
            result.ShouldBe("a");

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", 1 } });
            result.ShouldBe("a");

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", 3 } });
            result.ShouldBe("abc");

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", 9 } });
            result.ShouldBe("abc");
        }
    }
}