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

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }

        public int? Compare(object a, object b)
        {
            if (a == null && b == null)
                return 0;

            if (a == null)
                return -1;

            if (b == null)
                return 1;

            if (a is DateTime || b is DateTime)
            {
                var at = a is DateTime ? (DateTime)a : DateTime.Parse(a.ToString());
                var bt = b is DateTime ? (DateTime)b : DateTime.Parse(b.ToString());

                return DateTime.Compare(at, bt);
            }

            if (a is string || b is string)
            {
                return string.Compare(a.ToString(), b.ToString());
            }

            return decimal.Compare(a.TryToDecimal(), b.TryToDecimal());
        }
    }
}
