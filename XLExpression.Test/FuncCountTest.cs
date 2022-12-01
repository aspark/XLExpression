using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncCountTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("Count(1,0)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();
            result.ShouldBe(2);

            exp = ExpressionBuilder.Instance.Build("Count(0)");
            result = exp.Invoke();
            result.ShouldBe(1);

            exp = ExpressionBuilder.Instance.Build("Count(0, \"a\")");
            result = exp.Invoke();
            result.ShouldBe(1);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("Count(F2:G3, E2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "E2", 1 }, { "F2", 1 }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            result.ShouldBe(5);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", "a" }, { "F2", 1 }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            result.ShouldBe(4);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", "a" }, { "F2", "b" }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            result.ShouldBe(3);

            exp = ExpressionBuilder.Instance.Build("Count(F2:G2, F3:G3, E2)");
            result = exp.Invoke(new Dictionary<string, object> { { "E2", "a" }, { "F2", "b" }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            result.ShouldBe(3);

            exp = ExpressionBuilder.Instance.Build("Count(F2:G2, F3:G3, E2)");
            result = exp.Invoke(new Dictionary<string, object> { { "E2", "a" }, { "F2", 1 }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            result.ShouldBe(4);
        }
    }
}