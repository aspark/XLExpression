using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncOrTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("OR(1,0)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("OR(0,0)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("OR(0)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("OR(-1)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("OR(1)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("OR(10)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("OR(true)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("OR(false, 1)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("OR(false, true)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("OR(false, false)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("OR(false, true, 1, 0)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("OR(false, 10, 0)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("OR(false, 0, 0)");
            result = exp.Invoke();
            result.ShouldBe(false);
        }

        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("OR(F2,0)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 } });
            result.ShouldBe(true);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", 0 } });
            result.ShouldBe(false);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", true } });
            result.ShouldBe(true);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", false } });
            result.ShouldBe(false);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("OR(F2<G2, 0, 0)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 } });
            result.ShouldBe(true);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", 3 }, { "G2", 2 } });
            result.ShouldBe(false);
        }
    }
}