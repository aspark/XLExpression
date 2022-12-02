using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "round")]
    internal class FuncRound : FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args.Length == 2)
            {
                var precision = Convert.ToInt32(args[1]);

                var factor = (decimal)Math.Pow(10, precision);

                return Math.Round(args[0].TryToDecimal() * factor, MidpointRounding.AwayFromZero) / factor;
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
