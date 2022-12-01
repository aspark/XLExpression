using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression
{
    internal class XLException : Exception
    {
        public XLException()
        {

        }

        public XLException(string msg):base(msg)
        {

        }
    }
}
