using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "or")]
    internal class FuncOr : FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args?.Length > 0)
            {
                foreach (var arg in args)
                {
                    if (bool.Equals(arg, true) || arg.TryToInt() != 0)
                        return true;
                }

                return false;
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
