using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "sum")]
    internal class FuncSum: FunctionBase, IFunction
    {
        public object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = base.UnwarpArgs(dataContext, args);

            if (args?.Length == 2)
            {
                
            }

            throw new ArgumentException("参数错误");
        }
    }
}
