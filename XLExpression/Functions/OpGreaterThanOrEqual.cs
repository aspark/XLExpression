using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", ">=")]
    internal class OpGreaterThanOrEqual : IFunction
    {
        public object? Invoke(object[] args)
        {
            if (args?.Length == 2)
            {
                return Convert.ToDecimal(args[0]) >= Convert.ToDecimal(args[1]);
            }

            throw new ArgumentException("参数错误");
        }
    }
}
