using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncDateTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("Date(\"2022\",\"11\",\"22\")");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(DateTime.Parse("2022-11-22"));
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("Date(F1, F2, F3)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { F1 = 2022, F2 = "11", F3 = 22 });
            result.ShouldBe(DateTime.Parse("2022-11-22"));

            result = exp.Invoke(new { F1 = 2022, F2 = "11", F3 = "01" });
            result.ShouldBe(DateTime.Parse("2022-11-01"));
        }
    }
}