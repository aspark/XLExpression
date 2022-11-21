using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "<")]
    internal class FuncOpLessThan : FuncOpCompareBase, IFunction
    {
        public override object? Invoke(IFunctionDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            return base.Compare2Args(args) < 0;
        }
    }
}
