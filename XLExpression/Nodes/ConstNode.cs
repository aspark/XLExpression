using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression.Nodes
{
    internal class ConstNode : ExpressionNode
    {
        public ConstNode()
        {
            Type = NodeType.Const;
        }

        public object Value { get; set; }

    }
}
