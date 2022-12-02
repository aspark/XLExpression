using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression
{
    internal class NAError : XLException
    {
        public NAError()
        {

        }

        public NAError(string msg):base(msg)
        {

        }
    }
}
