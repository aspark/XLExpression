using XLExpression.Nodes;
using XLParser;

namespace XLExpression.Test
{
    public class BuilderTest
    {
        [Fact]
        public void ConvertToNodeTest1()
        {
            var node = ExpressionBuilder.Instance.ConvertToNode("1+2");

            node.ShouldNotBeNull();
            node.Type.ShouldBe(NodeType.Function);
            (node as FunctionNode)!.Name.ShouldBe("+");
            (node as FunctionNode)!.Arguments.Count.ShouldBe(2);
            (node as FunctionNode)!.Arguments[1]!.Type.ShouldBe(NodeType.Const);
        }

        [Fact]
        public void ConvertToNodeTest2()
        {
            var node = ExpressionBuilder.Instance.ConvertToNode("IF(F2>G2,1,0)");

            node.ShouldNotBeNull();
            node.Type.ShouldBe(NodeType.Function);
            (node as FunctionNode)!.Name.ShouldBe("if", StringCompareShould.IgnoreCase);
            (node as FunctionNode)!.Arguments.Count.ShouldBe(3);
            (node as FunctionNode)!.Arguments[0]!.Type.ShouldBe(NodeType.Function);
            ((node as FunctionNode)!.Arguments[0] as FunctionNode)!.Arguments[0]!.Type.ShouldBe(NodeType.Ref);
            (((node as FunctionNode)!.Arguments[0] as FunctionNode)!.Arguments[0]! as RefNode)!.Name.ShouldBe("F2");
        }

        [Fact]
        public void ConvertToNodeTest3()
        {
            var node = ExpressionBuilder.Instance.ConvertToNode("IF(Sheet1!$F$2>G2,1,0)");

            node.ShouldNotBeNull();
            node.Type.ShouldBe(NodeType.Function);
            (node as FunctionNode)!.Name.ShouldBe("if", StringCompareShould.IgnoreCase);
            (node as FunctionNode)!.Arguments.Count.ShouldBe(3);
            (node as FunctionNode)!.Arguments[0]!.Type.ShouldBe(NodeType.Function);
            ((node as FunctionNode)!.Arguments[0] as FunctionNode)!.Arguments[0]!.Type.ShouldBe(NodeType.Ref);
            (((node as FunctionNode)!.Arguments[0] as FunctionNode)!.Arguments[0]! as RefNode)!.Name.ShouldBe("Sheet1!F2");
        }

        [Fact]
        public void ComplexFormulaTest()
        {
            var exp = ExpressionBuilder.Instance.Build("ROUND(2*(1+B1)*(1+C1)+D1+IF(E1<=DATE($M$1-1,6,30),(F1+G1)*(1+H1)^2+I1+J1,IF(K1>0,K1+1*(1+B1)*(5+C1)/(1-N1)*N1,IF(OR(N1=0,N1=0.5),1*(1+B1)*(L1-(5+C1)),1*(1+B1)*(5+C1)/(1-N1)*N1))),-1)");
            var result = exp.Invoke(new {
                A1 = 0,
                B1 = 1,
                C1 = 2,
                D1 = 3,
                E1 = "2022-12-20",
                F1 = 5,
                G1 = 6,
                H1 = 7,
                I1 = 8,
                J1 = 9,
                K1 = 10,
                L1 = 11,
                M1 = 2022,
                N1 = 12,
            });

            result.ShouldBe(10);
        }
    }
}