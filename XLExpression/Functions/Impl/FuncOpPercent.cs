using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "%")]
    internal class FuncOpPercent : FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            //args = base.UnwarpArgs(dataContext, args);

            if (args?.Length == 1)
            {
                var value = args[0].TryToDecimal();

                return value / 100;
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
