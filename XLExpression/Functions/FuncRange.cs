using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", ":")]
    internal class FuncRange : FunctionBase, IFunction
    {
        public object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            //args = base.UnwarpArgs(dataContext, args);

            if (args?.Length == 2)
            {
                var start = (args[0] is FuncRefArg arg1) ? arg1.Name : args[0].ToString();
                var end = (args[1] is FuncRefArg arg2) ? arg2.Name : args[1].ToString();

                return dataContext[start + ":" + end];
            }

            throw new ArgumentException("参数错误");
        }
    }
}
