using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression
{
    public class NAError : XLException
    {
        public NAError()
        {

        }

        public NAError(string msg):base(msg)
        {

        }
    }
}
