using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "-")]
    internal class FuncOpMinus : FunctionBase, IFunction
    {
        public override object? Invoke(IDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args.Length == 1)
            {
                //取负
                return 0 - args[0].TryToDecimal();
            }
            else if (args.Length == 2)
            {
                return args[0].TryToDecimal() - args[1].TryToDecimal();
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
