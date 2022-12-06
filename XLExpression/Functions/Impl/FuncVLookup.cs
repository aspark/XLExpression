using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq.Expressions;
using System.Text;

namespace XLExpression.Functions.Impl
{
    [Export(typeof(IFunction))]
    [ExportMetadata("Symbol", "vlookup")]
    internal class FuncVLookup : FunctionBase, IFunction
    {
        public override object? Invoke(IDataContext dataContext, object[] args)
        {
            var realArgs = UnwarpArgs(dataContext, args);

            if (realArgs?.Length >= 2)
            {
                if (realArgs[1] is object[,] array)
                {
                    var value = realArgs[0];
                    var stringValue = realArgs[0].ToStringOrEmpty();
                    var index = realArgs[2].TryToInt() - 1;

                    if (index < 0)
                        index = 0;

                    if(index > array.GetLength(1))
                        throw new ArgumentException("参数错误:" + this.GetType().Name + $"，索引{index}超出范围");

                    var rangeLookup = realArgs.Length > 3 ? realArgs[3].TryToBool() : true;
                    for (int r = 0; r < array.GetLength(0); r++)
                    {
                        if (rangeLookup)
                        {
                            if (array[r, 0].ToStringOrEmpty().IndexOf(stringValue) > -1)
                            {
                                return array[r, index];
                            }
                        }
                        else
                        {
                            if (array[r, 0].Compare(value) == 0)
                                return array[r, index];
                        }
                    }

                    return null;//can not find the value
                }
            }

            throw new ArgumentException("参数错误:" + this.GetType().Name);
        }
    }
}
