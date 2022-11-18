using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "*")]
    internal class OpMul : IFunction
    {
        public object? Invoke(object[] args)
        {
            return args;
        }
    }
}
