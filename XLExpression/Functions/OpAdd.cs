using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace XLExpression.Functions
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "+")]
    internal class OpAdd : IFunction
    {
        public object? Invoke(object[] args)
        {
            if (args.Length == 1)
            {
                //原样返回
                return args[0];
            }
            else if (args.Length == 2)
            {
                if(args[0] is string || args[1] is string)
                {
                    return args[0].ToString() + args[1].ToString();
                }

                return Convert.ToDecimal(args[0]) + Convert.ToDecimal(args[1]);
            }

            return null;
        }
    }
}
