using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression.Nodes
{
    internal class RefNode : ExpressionNode
    {
        public RefNode()
        {
            Type = NodeType.Ref;
        }

        public string Name { get; set; }
    }
}
