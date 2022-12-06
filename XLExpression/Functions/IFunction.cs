using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace XLExpression.Functions
{
    public interface IFunction
    {
        object? Invoke(IDataContext dataContext, object[] args);
    }
    public interface IFunctionMetadata
    {
        string Symbol { get; }
    }


    /// <summary>
    /// 传入的是地址引用参数,哪：F2
    /// </summary>
    internal class FuncRefArg
    {
        public FuncRefArg(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }

        public bool IsRange => Name?.Contains(':') == true;
    }


    internal abstract class FunctionBase: IFunction
    {
        //因每个函数对参数的使用方式不一样
        //所以，由个函数自身处理参数判断。不在基类统一处理，如：Sum(a, b)，如果统一处理，需要将参数展开
        public abstract object? Invoke(IDataContext dataContext, object[] args);

        public IInvokeContext CurrentInvokeContext => DefaultInvokeContext.Current;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="args"></param>
        /// <param name="isRange">如果是Range，只解析开始与结束两个参数</param>
        /// <returns></returns>
        public object[] UnwarpArgs(IDataContext dataContext, object[] args)//, bool isRange = false
        {
            if (args == null)
                return new object[0];

            var unWrapped = new object?[args.Length];

            for(var i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                if (arg is FuncRefArg refArg)
                {
                    var name = refArg.Name.Replace("$", "");//remove $

                    unWrapped[i] = dataContext[name];
                }
                else
                {
                    unWrapped[i] = arg;
                }
            }

            return unWrapped;
        }

    }

}
