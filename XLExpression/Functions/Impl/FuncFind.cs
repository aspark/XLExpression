using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "find")]
    internal class FuncFind : FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args?.Length >= 2)//char, num
            {
                var findText = args[0].ToString();
                var withinText = args[1].ToString();
                //var num = args.Length > 1 ? (args[1].TryToNullableInt() ?? 1) : 1;
                var start = (args.Length > 2 ? args[1] != null ? args[2].TryToInt() : 1 : 1);
                if (start < 0)
                {
                    throw new ArgumentException("参数错误:" + this.GetType().Name + $"start;{start}");
                }

                if (string.IsNullOrEmpty(findText))
                    return 1;

                if (string.IsNullOrEmpty(withinText) || start >= withinText.Length)
                {
                    return null;
                }

                var index = withinText.IndexOf(findText, start - 1);

                if (index < 0)
                    return null;

                return index + 1;
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
