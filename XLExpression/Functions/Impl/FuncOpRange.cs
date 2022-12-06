using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", ":")]
    internal class FuncOpRange : FunctionBase, IFunction
    {
        public override object? Invoke(IDataContext dataContext, object[] args)
        {
            var realArgs = base.UnwarpArgs(dataContext, args);

            if (args?.Length == 2)
            {
                var start = args[0] is FuncRefArg arg1 ? arg1.Name : realArgs[0].ToString();
                var end = args[1] is FuncRefArg arg2 ? arg2.Name : realArgs[1].ToString();
                var name = start + ":" + end;

                var result = dataContext[name];

                base.CurrentInvokeContext?.MapResultToRange(result, name);

                return result;
            }
            else if (args?.Length == 1)
            {
                var cell = args[0] is FuncRefArg arg1 ? arg1.Name : realArgs[0].ToString();

                return dataContext[cell];
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
