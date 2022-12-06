using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "^")]
    internal class FuncOpPow : FunctionBase, IFunction
    {
        public override object? Invoke(IDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args.Length == 2)
            {
                return Math.Pow(args[0].TryToDouble(), args[1].TryToDouble());
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
