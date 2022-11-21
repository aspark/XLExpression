using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using XLExpression.Functions;

namespace XLExpression
{
    public class ExpressionResult
    {
        public ExpressionResult(Expression? exp)
        {
            Exp = exp;
        }

        public Expression Exp { get; private set; }

        /// <summary>
        /// ordered args
        /// </summary>
        public string[]? ArgNames => Args?.Select(r => r.Name).ToArray();

        internal ParameterExpression[] Args { get; set; }

        public bool HasRel { get; internal set; }

        public object Invoke(Dictionary<string, object>? args = null)
        {
            return this.Invoke(args != null ? new FunctionDataContext(args!) : null);
        }

        public object Invoke(IFunctionDataContext? dataContext)
        {
            if (Exp.NodeType == ExpressionType.Call)
            {
                if (Args?.Length > 0)
                {
                    //if (args?.Count != Args.Length || Args.Any(arg => !args.ContainsKey(arg.Name)))
                    //{
                    //    throw new ArgumentException("miss argument");
                    //}

                    var lambda = Expression.Lambda(Exp, Args);

                    return lambda.Compile().DynamicInvoke(Args.Select(arg => {
                        if(arg.Type == typeof(IFunctionDataContext))
                        {
                            return dataContext;
                        }
                        else if(arg.Type == typeof(FuncRefArg))
                        {
                            return new FuncRefArg(arg.Name);
                        }

                        return (object?)null;
                    }).ToArray());
                }
            }

            return Expression.Lambda<Func<object>>(Exp).Compile()();
        }
    }
}
