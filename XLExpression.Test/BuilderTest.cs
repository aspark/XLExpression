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
    }
}