using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "Percentile")]
    internal class FuncPercentile : FuncPercentileBase//FunctionBase, IFunction
    {
        public override object? Invoke(IDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args?.Length >= 2)//char, num
            {
                decimal[]? range = args[0] is object[,] r ? r.Flat(a => a.TryToNullableDecimal()).Where(a => a.HasValue).Select(a => a.Value).ToArray() : null;
                if (range == null)
                {
                    throw new NumError("Percentile参数 range 不是数组");
                }

                var k = args[1].TryToDecimal(false);//float
                if (k > 1 || k < 0)
                {
                    throw new NumError("Percentile参数 k 不在[0,1]范围内");
                }

                return Calc(range, k);
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }

    internal abstract class FuncPercentileBase : FunctionBase, IFunction
    {
        protected decimal Calc(decimal[] array, decimal k, bool include01 = true)
        {
            if (array == null || array.Length == 0 || k < 0 || k > 1)
            {
                throw new ArgumentException("invalid arguments");
            }

            Array.Sort(array);

            if (k == 0)
                return array[0];

            if (k == 1)
                return array[array.Length - 1];

            decimal position;
            if (include01)
                position = (array.Length - 1) * k;
            else
            {
                decimal exclude = 1.0M / (array.Length + 1);
                if (k < exclude || k > array.Length * exclude)
                    throw new NumError("Percentile参数 k 不在允许范围内");

                position = (array.Length + 1) * k - 1;
            }

            var sPostion = (int)Math.Truncate(position);
            var rPosition = position - sPostion;

            decimal a = array[sPostion];

            if (rPosition == 0)
                return a;

            decimal b = array[sPostion + 1];

            return a + (b - a) * rPosition;
        }
    }
}
