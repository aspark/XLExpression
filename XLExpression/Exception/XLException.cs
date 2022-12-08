using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression
{
    public class XLException : Exception
    {
        public XLException()
        {

        }

        public XLException(string msg):base(msg)
        {

        }
    }
}
