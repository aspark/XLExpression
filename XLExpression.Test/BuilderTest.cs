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
            (node as FunctionNode).Name.ShouldBe("+");
            (node as FunctionNode).Arguments.Count.ShouldBe(2);
            (node as FunctionNode).Arguments[1].Type.ShouldBe(NodeType.Const);
        }

        [Fact]
        public void ConvertToNodeTest2()
        {
            var node = ExpressionBuilder.Instance.ConvertToNode("IF(F2>G2,1,0)");

            node.ShouldNotBeNull();
            node.Type.ShouldBe(NodeType.Function);
            (node as FunctionNode).Name.ShouldBe("if", StringCompareShould.IgnoreCase);
            (node as FunctionNode).Arguments.Count.ShouldBe(3);
            (node as FunctionNode).Arguments[0].Type.ShouldBe(NodeType.Function);
        }

        [Fact]
        public void CompliTest()
        {
            var exp = ExpressionBuilder.Instance.Build("ROUND(AI2*(1+CT2)*(12+AL2)+CN2+IF(J2<=DATE($U$5-2,12,31),(CD2+CE2)*(1+CU2)^2+CF2+CG2,IF(CJ2>0,CJ2+AI2*(1+CT2)*(12+AL2)/(1-AN2)*AN2,IF(OR(AN2=0,AN2=0.0714),AI2*(1+CT2)*(CS2-(12+AL2)),AI2*(1+CT2)*(12+AL2)/(1-AN2)*AN2))),-1)");
            var result = exp.Invoke(new { });

            result.ShouldNotBeNull();
        }
    }
}