using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "sum")]
    internal class FuncSum : FunctionBase, IFunction
    {
        public override object? Invoke(IDataContext dataContext, object[] args)
        {
            var realArgs = UnwarpArgs(dataContext, args);

            if (realArgs?.Length > 0)
            {
                decimal sum = 0;
                foreach (var arg in realArgs)
                {
                    if (arg is object[,] range)
                    {
                        for (var r = 0; r < range.GetLength(0); r++)
                        {
                            for (var c = 0; c < range.GetLength(1); c++)
                            {
                                sum += range[r, c].TryToDecimal();
                            }
                        }
                    }
                    else
                    {
                        sum += arg.TryToDecimal();
                    }
                }

                return sum;
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
