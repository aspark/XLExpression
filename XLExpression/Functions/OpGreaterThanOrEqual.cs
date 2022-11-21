using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", ">=")]
    internal class OpGreaterThanOrEqual : FunctionBase, IFunction
    {
        public object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = base.UnwarpArgs(dataContext, args);

            if (args?.Length == 2)
            {
                return Convert.ToDecimal(args[0]) >= Convert.ToDecimal(args[1]);
            }

            throw new ArgumentException("参数错误");
        }
    }
}
