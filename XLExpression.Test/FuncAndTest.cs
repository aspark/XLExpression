using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncAndTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("And(1,0)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("And(0,0)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("And(1,1)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("And(0)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("And(-1)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("And(1)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("And(10)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("And(true)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("And(false)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("And(false, 1)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("And(true, 1)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("And(true, true)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("And(true, false)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("And(false, true, 1, 0)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("And(true, 1, 1)");
            result = exp.Invoke();
            result.ShouldBe(true);

            exp = ExpressionBuilder.Instance.Build("And(false, 0, 1)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("And(1, 1, false)");
            result = exp.Invoke();
            result.ShouldBe(false);

            exp = ExpressionBuilder.Instance.Build("And(-1, 10, true)");
            result = exp.Invoke();
            result.ShouldBe(true);
        }

        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("And(F2,1)");

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
            var exp = ExpressionBuilder.Instance.Build("And(F2<G2, 1, 1)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 } });
            result.ShouldBe(true);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", 3 }, { "G2", 2 } });
            result.ShouldBe(false);
        }
    }
}