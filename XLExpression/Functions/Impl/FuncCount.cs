using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "count")]
    internal class FuncCount : FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args?.Length >= 1)//char, num
            {
                var count = 0;

                foreach (var arg in args)
                {
                    if(arg is object[,] map)
                    {
                        count += map.Flat().Count(i => i.IsNumber());
                    }
                    else if(arg.IsNumber())
                    {
                        count++;
                    }
                }

                return count;
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
