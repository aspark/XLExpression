using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncIfTest
    {
        //IF
        [Fact]
        public void IfTest1()
        {
            var exp = ExpressionBuilder.Instance.Build("IF(F2>G2,1,0)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 } });

            result.ShouldBe(0);
        }

        [Fact]
        public void IfTest2()
        {
            var exp = ExpressionBuilder.Instance.Build("IF(F2<G2,1,0)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 } });

            result.ShouldBe(1);
        }
    }
}