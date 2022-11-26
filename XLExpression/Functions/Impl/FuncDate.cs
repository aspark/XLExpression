using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "date")]
    internal class FuncDate : FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args.Length == 3)
            {
                return new DateTime(args[0].TryToInt(), args[1].TryToInt(), args[2].TryToInt());
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
