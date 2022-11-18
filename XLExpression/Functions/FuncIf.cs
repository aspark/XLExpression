using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "if")]
    internal class FuncIf: IFunction
    {
        public object? Invoke(object[] args)
        {
            if (args?.Length == 3)
            {
                var b = false;
                if (args[0] is bool)
                {
                    b = (bool)args[0];
                }
                else if (args[0] is Expression)
                {

                }
                else if (args[0] != null)
                {
                    b = true;
                }

                return b ? args[1] : args[2];
            }

            throw new ArgumentException("参数错误");
        }
    }
}
