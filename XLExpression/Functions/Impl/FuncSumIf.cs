using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "sumif")]
    internal class FuncSumIf : FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args?.Length >= 2)
            {
                object[,]? range = args[0] is object[,] r ? r : null;
                object[,]? criteria = args[1] is object[,] c ? c : new object[1, 1] { { args[1].ToString() } };
                object[,]? sumRange = args.Length > 2 ? (args[2] is object[,] s) ? s : range : range;

                var sumRangeRows = sumRange.GetLength(0);
                var sumRangeCols = sumRange.GetLength(1);

                if (sumRangeRows < range.GetLength(0) || sumRangeCols < range.GetLength(1))//excel会取未在的sumrange范围中的值
                {
                    throw new NotImplementedException("not support sum range less than range");
                }

                var result = new object[criteria.GetLength(0), criteria.GetLength(1)];

                for (var i = 0; i < criteria.GetLength(0); i++)
                {
                    for (var j = 0; j < criteria.GetLength(1); j++)
                    {
                        decimal sum = 0;

                        var value = criteria[i, j]?.ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            continue;
                        }

                        range.Visit((o, i) => {
                            if (o.IsMatch(value))
                            {
                                //if(i.Row>=sumRangeRows || i.Col >= sumRangeRows)
                                //{
                                //    sum += 0;//todo:从dataContext来
                                //}
                                //else
                                //{
                                    sum += sumRange[i.Row, i.Col].TryToDecimal();
                                //}
                            }
                        });

                        result[i, j] = sum;

                    }
                }

                return result.Length == 1 ? (object)result[0, 0] : result;
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
