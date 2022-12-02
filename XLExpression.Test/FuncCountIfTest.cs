using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncCountIfTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("CountIf(F2:G3, E2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "E2", 1 }, { "F2", 1 }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            result.ShouldBe(2);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", "a" }, { "F2", 1 }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            result.ShouldBe(0);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", "" }, { "F2", "b" }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            result.ShouldBe(0);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", "b" }, { "F2", "b" }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            result.ShouldBe(1);

            result = exp.Invoke(new Dictionary<string, object> { { "E2", ">a" }, { "F2", "b" }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            result.ShouldBe(1);

            exp = ExpressionBuilder.Instance.Build("CountIf(F2:G3, D2:E2)");
            result = exp.Invoke(new Dictionary<string, object> { { "D2", 1 }, { "E2", ">a" }, { "F2", "b" }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            (result is object[,]).ShouldBeTrue();
            (result as object[,]).GetLength(0).ShouldBe(1);
            (result as object[,]).GetLength(1).ShouldBe(2);
            (result as object[,])[0, 0].ShouldBe(1);
            (result as object[,])[0, 1].ShouldBe(1);

            exp = ExpressionBuilder.Instance.Build("CountIf(F2:G3, D2:E3)");
            result = exp.Invoke(new Dictionary<string, object> { { "D2", 1 }, { "E2", ">a" }, { "D3", 2 }, { "E3", ">b" }, { "F2", "b" }, { "G2", 2 }, { "F3", 1 }, { "G3", 2 } });
            (result is object[,]).ShouldBeTrue();
            (result as object[,]).GetLength(0).ShouldBe(2);
            (result as object[,]).GetLength(1).ShouldBe(2);
            (result as object[,])[0, 0].ShouldBe(1);
            (result as object[,])[0, 1].ShouldBe(1);
            (result as object[,])[1, 0].ShouldBe(2);
            (result as object[,])[1, 1].ShouldBe(0);
        }
    }
}