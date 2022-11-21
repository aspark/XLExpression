using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "-")]
    internal class OpMinus : FunctionBase, IFunction
    {
        public object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = base.UnwarpArgs(dataContext, args);

            if (args.Length == 1)
            {
                //取负
                return 0 - args[0].TryToDecimal();
            }
            else if (args.Length == 2)
            {
                return args[0].TryToDecimal() - args[1].TryToDecimal();
            }

            return null;
        }
    }
}
