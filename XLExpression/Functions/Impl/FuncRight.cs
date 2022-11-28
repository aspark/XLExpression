using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "right")]
    internal class FuncRight : FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args?.Length > 0)//char, num
            {
                var text = args[0].ToString();
                //var num = args.Length > 1 ? (args[1].TryToNullableInt() ?? 1) : 1;
                var num = args.Length > 1 ? args[1] != null ? args[1].TryToInt() : 1 : 1;
                if (num < 0)
                {
                    throw new ArgumentException("参数错误:" + this.GetType().Name + $"num;{num}");
                }

                if (num == 0)
                    return "";

                if (num > text.Length)
                    return text;

                return text.Substring(text.Length - num, num);
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
