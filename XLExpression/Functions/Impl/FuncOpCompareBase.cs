using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions.Impl
{
    internal abstract class FuncOpCompareBase : FunctionBase
    {
        public int? Compare2Args(object[] args)
        {
            if (args?.Length == 2)
            {
                return args[0].Compare(args[1]);
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
