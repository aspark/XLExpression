using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "Percentile.Inc")]
    internal class FuncPercentileInc : FuncPercentile//FunctionBase, IFunction
    {
        
    }
}
