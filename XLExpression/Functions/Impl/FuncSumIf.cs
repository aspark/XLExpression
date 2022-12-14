using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using XLExpression.Common;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "sumif")]
    internal class FuncSumIf : FunctionBase, IFunction
    {
        public override object? Invoke(IDataContext dataContext, object[] args)
        {
            var oldArgs = args;
            args = UnwarpArgs(dataContext, args);

            if (args?.Length >= 2)
            {
                object[,]? range = args[0] is object[,] r ? r : null;
                
                if(range == null)
                {
                    throw new ValueError("range is null");
                }
                
                object?[,] criteria = args[1] is object[,] c ? c : new object[1, 1] { { args[1].ToString() } };
                object?[,] sumRange = args.Length > 2 ? (args[2] is object[,] s) ? s : range : range;

                var rangeRows = range.GetLength(0);
                var rangeCols = range.GetLength(1);

                if (sumRange.GetLength(0) < rangeRows || sumRange.GetLength(1) < rangeCols)//excel会取未在的sumrange范围中的值
                {
                    object?[,] getRange(string name)
                    {
                        var pos = name.Split(":").Select(ExcelHelper.ConvertNameToPosition).ToArray(); //.Replace("$", "")
                        var start = pos.First();
                        var end = pos.Last();

                        var rowStart = start.Row ?? 0;
                        var colStart = start.Col ?? 0;
                        var rowCount = rangeRows;// end.Row.HasValue ? end.Row.Value - rowStart + 1 : dataContext.RowCount;
                        var colCount = rangeCols;// end.Col.HasValue ? end.Col.Value - colStart + 1 : dataContext.ColCount;

                        return dataContext[rowStart, rowCount, colStart, colCount];
                    }

                    if(oldArgs[2] is FuncRefArg refArg)
                    {
                        sumRange = getRange(refArg.Name);
                    }
                    else
                    {
                        //从执行上下文中获取range的解析结果
                        var name = base.CurrentInvokeContext?.GetResultRange(oldArgs[2]);
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            sumRange = getRange(name);
                        }
                        else
                            throw new NotImplementedException("not support sum range less than range");
                    }
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
                                sum += sumRange[i.Row, i.Col].TryToDecimal();
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
