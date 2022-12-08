using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression
{
    public class NumError : XLException
    {
        public NumError()
        {

        }

        public NumError(string msg):base(msg)
        {

        }
    }
}
