using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression.Nodes
{
    internal class FunctionNode: ExpressionNode
    {
        public FunctionNode()
        {
            Type = NodeType.Function;
        }

        public string Name { get; set; }



        public List<ExpressionNode?> Arguments { get; set; } = new List<ExpressionNode?>();
    }
}
