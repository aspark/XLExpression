using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "CountIf")]
    internal class FuncCountIf : FunctionBase, IFunction
    {
        public override object? Invoke(IDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args?.Length >= 2)//range, range
            {
                string[]? range = args[0] is object[,] r ? r.Flat(a => a?.ToString()).Where(a => !string.IsNullOrEmpty(a)).ToArray() : null;
                if (range == null)
                {
                    throw new NumError("Percentile参数 range 不是数组");
                }

                //todo:支持数组函数类型
                object[,]? criteria = args[1] is object[,] c ? c : new object[1, 1] { { args[1].ToString() } };

                var result = new object[criteria.GetLength(0), criteria.GetLength(1)];

                for(var i= 0;i< criteria.GetLength(0); i++)
                {
                    for (var j = 0; j < criteria.GetLength(1); j++)
                    {
                        var value = criteria[i, j]?.ToString();
                        if (string.IsNullOrEmpty(value))
                        {
                            result[i, j] = 0;
                            continue;
                        }

                        //value 可能符号前缀
                        result[i, j] = range.Count(i => i.IsMatch(value));

                    }
                }

                return result.Length == 1 ? (object)result[0, 0] : result;
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
