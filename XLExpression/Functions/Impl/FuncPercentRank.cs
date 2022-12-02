using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;
using System.Linq;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "PercentRank")]
    internal class FuncPercentRank : FuncPercentRankBase //FunctionBase, IFunction
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

                return Calc(range, x, significance);
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }

    internal abstract class FuncPercentRankBase : FunctionBase, IFunction
    {
        protected decimal Calc(decimal[] array, decimal x, int significance = 3, bool include01 = true)
        {
            if (array == null || array.Length == 0)
            {
                throw new ValueError("invalid arguments");
            }

            Array.Sort(array);

            if (x < array[0] || x > array[array.Length - 1])
                throw new NAError("x超出范围");

            var position = 0;
            for (; position < array.Length; position++)
            {
                if(array[position] > x)
                {
                    break;
                }
            }

            position--;

            //if (include01 == false)
            //{
            //    var exclude = 1.0 / (array.Length + 1);
            //}

            //significance = Math.Min(3, significance);

            decimal slot = 0;
            if (include01)
            {
                slot = 1.0M / (array.Length - 1);
            }
            else
            {
                slot = 1.0M / (array.Length + 1);
            }

            decimal k = (position + (include01 ? 0 : 1)) * slot;
            if (array[position] != x)
            {
                k = k + (x - array[position]) / (array[position + 1] - array[position])* slot;
            }

            decimal tmp = (decimal)Math.Pow(10, significance);


            return Math.Truncate(k * tmp) / tmp;//截断的 //Math.Round
        }
    }
}
