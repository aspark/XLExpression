using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class FuncVLookupTest
    {
        [Fact]
        public void Test1()
        {
            var exp = ExpressionBuilder.Instance.Build("VLOOKUP(\"21\", F2:H3, 2)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { 
                F2 = 12, G2 = 2, H2 = 3, 
                F3 = 21, G3 = 12, H3 = 23 
            });

            result.ShouldBe(12);
        }

        [Fact]
        public void Test2()
        {
            var exp = ExpressionBuilder.Instance.Build("VLOOKUP(A1, F2:H3, 2, false)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new { 
                A1 = 21,
                F2 = 12, G2 = 2, H2 = 3, 
                F3 = 21, G3 = 12, H3 = 23 
            });

            result.ShouldBe(12);
        }

        [Fact]
        public void Test3()
        {
            var exp = ExpressionBuilder.Instance.Build("VLOOKUP(A1, F2:H3, I1, J1)");

            exp.ShouldNotBeNull();
            exp.Exp.ShouldNotBeNull();

            var result = exp.Invoke(new
            {
                A1 = 21,
                I1 = 1,
                J1 = true,
                F2 = 121, G2 = 2, H2 = 3, 
                F3 = 21, G3 = 12, H3 = 23 
            });
            result.ShouldBe(121);

            result = exp.Invoke(new
            {
                A1 = 21,
                I1 = 1,
                J1 = false,
                F2 = 121, G2 = 2, H2 = 3, 
                F3 = 21, G3 = 12, H3 = 23 
            });
            result.ShouldBe(21);


            result = exp.Invoke(new
            {
                A1 = 21,
                I1 = 2,
                J1 = false,
                F2 = 121, G2 = 2, H2 = 3, 
                F3 = 21, G3 = 12, H3 = 23 
            });
            result.ShouldBe(12);

            result = exp.Invoke(new
            {
                A1 = 1,
                I1 = 2,
                J1 = true,
                F2 = 12, G2 = 2, H2 = 3, 
                F3 = 21, G3 = 12, H3 = 23 
            });
            result.ShouldBe(2);

            result = exp.Invoke(new
            {
                A1 = 1,
                I1 = 3,
                J1 = true,
                F2 = 12, G2 = 2, H2 = 3, 
                F3 = 21, G3 = 12, H3 = 23 
            });
            result.ShouldBe(3);
        }

    }
}