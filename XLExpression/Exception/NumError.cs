using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression
{
    internal class NumError : XLException
    {
        public NumError()
        {

        }

        public NumError(string msg):base(msg)
        {

        }
    }
}
