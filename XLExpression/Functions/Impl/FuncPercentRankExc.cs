using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "PercentRank.Exc")]
    internal class FuncPercentRankExc : FuncPercentRankBase// FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args?.Length >= 2)//char, num
            {
                decimal[]? range = args[0] is object[,] r ? r.Flat(a => a.TryToNullableDecimal()).Where(a => a.HasValue).Select(a => a.Value).ToArray() : null;
                if (range == null)
                {
                    throw new NumError("PercentRank参数 range 不是数组");
                }

                var x = args[1].TryToDecimal(false);//float

                var significance = args.Length > 2 ? args[2].TryToInt() : 3;

                return Calc(range, x, significance, false);
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
