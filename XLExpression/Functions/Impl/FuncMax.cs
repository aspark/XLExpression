using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "MAX")]
    internal class FuncMax : FunctionBase, IFunction
    {
        public override object? Invoke(IDataContext dataContext, object[] args)
        {
            args = UnwarpArgs(dataContext, args);

            if (args.Length > 0)
            {
                var reals = args.Select(a => a.TryToNullableDecimal()).Where(a => a != null).Select(a => a.Value);
                if (reals.Any() == false)
                    return 0;

                return reals.Max();
            }

            throw new XLException("miss arguments");
        }
    }
}
