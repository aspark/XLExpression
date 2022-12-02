using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "PercentRank.Inc")]
    internal class FuncPercentRankInc : FuncPercentRank //FunctionBase, IFunction
    {

    }
}
