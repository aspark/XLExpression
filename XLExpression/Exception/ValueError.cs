using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression
{
    internal class ValueError : XLException
    {
        public ValueError()
        {

        }

        public ValueError(string msg):base(msg)
        {

        }
    }
}
