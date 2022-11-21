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
                return Compare(args[0], args[1]);
            }

            throw new ArgumentException("参数错误:Compare");
        }

        public int? Compare(object a, object b)
        {
            if (a == null && b == null)
                return 0;

            if (a == null)
                return -1;

            if (b == null)
                return 1;

            if(a is string || b is string)
            {
                string.Compare(a.ToString(), b.ToString());
            }

            return decimal.Compare(a.TryToDecimal(), b.TryToDecimal());
        }
    }
}
