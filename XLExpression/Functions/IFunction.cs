using System;
using System.Collections.Generic;
using System.Text;

namespace XLExpression.Functions
{
    public interface IFunction
    {
        object? Invoke(object[] args);
    }

    public interface IFunctionData
    {
        string Symbol { get; }
    }
}
