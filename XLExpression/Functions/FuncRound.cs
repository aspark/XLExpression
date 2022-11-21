using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "round")]
    internal class FuncRound : FunctionBase, IFunction
    {
        public object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = base.UnwarpArgs(dataContext, args);

            return args;
        }
    }
}
