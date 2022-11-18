using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

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

        public object Invoke(Dictionary<string , object>? args = null)
        {
            if(Exp.NodeType == ExpressionType.Call)
            {
                if (Args?.Length > 0)
                {
                    if (args?.Count != Args.Length || Args.Any(arg => !args.ContainsKey(arg.Name)))
                    {
                        throw new ArgumentException("miss argument");
                    }

                    //Expression<Func<int, int, object>> ex = (F2, G2) => FunctionFactory.Instance.GetOperator("IF").Invoke(new[] { FunctionFactory.Instance.GetOperator(">").Invoke(new object[] { F2, G2 }), 1, 0 });

                    var lambda = Expression.Lambda(Exp, Args);

                    return lambda.Compile().DynamicInvoke(ArgNames.Select(name => args[name]).ToArray());
                }
            }

            return Expression.Lambda<Func<object>>(Exp).Compile()();
        }
    }
}
