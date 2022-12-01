using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "Percentile.Exc")]
    internal class FuncPercentileExc : FuncPercentile//FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args?.Length >= 2)//char, num
            {
                double[]? range = args[0] is object[,] r ? r.Flat(a => a.TryToDouble()) : null;
                if (range == null)
                {
                    throw new NumError("Percentile参数 range 不是数组");
                }

                var k = args[1].TryToDouble(false);//float
                if (k >= 1 || k <= 0)
                {
                    throw new NumError("Percentile参数 k 不在(0,1)范围内");
                }

                return Calc(range, k, false);
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }

    }
}
