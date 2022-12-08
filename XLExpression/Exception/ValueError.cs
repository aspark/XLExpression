using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression
{
    public class ValueError : XLException
    {
        public ValueError()
        {

        }

        public ValueError(string msg):base(msg)
        {

        }
    }
}
