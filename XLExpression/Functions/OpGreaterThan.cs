using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", ">")]
    internal class OpGreaterThan : FunctionBase, IFunction
    {
        public object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = base.UnwarpArgs(dataContext, args);

            if (args?.Length == 2)
            {
                return args[0].TryToDecimal() > args[1].TryToDecimal();
            }

            throw new ArgumentException("参数错误");
        }
    }
}
