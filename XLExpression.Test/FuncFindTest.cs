using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncFindTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("Find(\"abc\", \"12abcd54abc\")");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();
            result.ShouldBe(3);

            exp = ExpressionBuilder.Instance.Build("Find(\"abc1\", \"12abcd54abc\")");
            result = exp.Invoke();
            result.ShouldBe(null);

            exp = ExpressionBuilder.Instance.Build("Find(\"abc\", \"12abcd54abc\", 4)");
            result = exp.Invoke();
            result.ShouldBe(9);

            exp = ExpressionBuilder.Instance.Build("Find(\"abc\", \"12abcd54abc\", 20)");
            result = exp.Invoke();
            result.ShouldBe(null);

            exp = ExpressionBuilder.Instance.Build("Find(\"\", \"12abcd54abc\")");
            result = exp.Invoke();
            result.ShouldBe(1);
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("Find(F2, \"12abcd54abc\")");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" } });
            result.ShouldBe(3);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc1" } });
            result.ShouldBe(null);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" } });
            result.ShouldBe(3);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "" } });
            result.ShouldBe(1);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("Find(F2, G2, H2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", "12abcd54abc" }, { "H2", 2 } });
            result.ShouldBe(3);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", "12abcd54abc" }, { "H2", 4 } });
            result.ShouldBe(9);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "abc" }, { "G2", "12abcd54abc" }, { "H2", 20 } });
            result.ShouldBe(null);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "" }, { "G2", "12abcd54abc" }, { "H2", 2 } });
            result.ShouldBe(1);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "c" }, { "G2", "12abcd54abc" }, { "H2", 6 } });
            result.ShouldBe(11);

            result = exp.Invoke(new Dictionary<string, object> { { "F2", "12abcd54abc" }, { "G2", "12abcd54abc" }, { "H2", 1 } });
            result.ShouldBe(1);
        }
    }
}