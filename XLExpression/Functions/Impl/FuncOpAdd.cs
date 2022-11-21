using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "+")]
    internal class FuncOpAdd : FunctionBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args.Length == 1)
            {
                //原样返回
                return args[0];
            }
            else if (args.Length == 2)
            {
                if (args[0] is string || args[1] is string)
                {
                    return args[0].ToString() + args[1].ToString();
                }

                return Convert.ToDecimal(args[0]) + Convert.ToDecimal(args[1]);
            }

            throw new ArgumentException("参数错误");
        }
    }
}
