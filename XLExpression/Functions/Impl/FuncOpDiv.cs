using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "/")]
    internal class FuncOpDiv : FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args.Length == 2)
            {
                var b = args[1].TryToDecimal();
                if (b == 0)
                    return double.NegativeInfinity;//infinite

                return args[0].TryToDecimal() / b;
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
