using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncRightTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("Right(\"abc\", 1)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();
            result.ShouldBe("c");

            exp = ExpressionBuilder.Instance.Build("Right(\"abc\", 2)");
            result = exp.Invoke();
            result.ShouldBe("bc");

            exp = ExpressionBuilder.Instance.Build("Right(\"abc\")");
            result = exp.Invoke();
            result.ShouldBe("c");
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("Right(F2,2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" } });
            result.ShouldBe("bc");
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("Right(F2,G2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", 2 } });
            result.ShouldBe("bc");

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" } });
            result.ShouldBe("c");

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", 1 } });
            result.ShouldBe("c");

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", 3 } });
            result.ShouldBe("abc");

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", 9 } });
            result.ShouldBe("abc");
        }
    }
}