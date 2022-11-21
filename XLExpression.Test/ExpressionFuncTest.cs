using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class ExpressionFuncTest
    {
        //ADD
        [Fact]
        public void AddTest1()
        {
            var exp = ExpressionBuilder.Instance.Build("1+2");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(3);
        }

        [Fact]
        public void AddTest2()
        {
            var exp = ExpressionBuilder.Instance.Build("1+2+3");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke();

            result.ShouldBe(6);
        }

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

        //SUM
        [Fact]
        public void SumTest1()
        {
            var exp = ExpressionBuilder.Instance.Build("SUM(F2:H2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 } });

            result.ShouldBe(6);
        }

        [Fact]
        public void SumTest2()
        {
            var exp = ExpressionBuilder.Instance.Build("SUM(F2:H3)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 11 }, { "G3", 12 }, { "H3", 13 } });

            result.ShouldBe(42);
        }

        [Fact]
        public void SumTest3()
        {
            var exp = ExpressionBuilder.Instance.Build("SUM(F2:H2, F3:H3)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new Dictionary<string, object> { { "F2", 1 }, { "G2", 2 }, { "H2", 3 }, { "F3", 11 }, { "G3", 12 }, { "H3", 13 } });

            result.ShouldBe(42);
        }

    }
}