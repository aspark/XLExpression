using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
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
        public string[]? ArgNames => Parameters?.Select(r => r.Name).ToArray();

        internal ParameterExpression[] Parameters { get; set; }

        public bool HasRel { get; internal set; }

        public object Invoke(Dictionary<string, object>? args = null)
        {
            return this.Invoke(args != null ? new DefaultDataContext(args!) : null);
        }

        public object Invoke(object args)
        {
            var ctx = new DefaultDataContext();
            foreach(PropertyDescriptor property in TypeDescriptor.GetProperties(args))
            {
                ctx[property.Name] = property.GetValue(args);
            }

            return this.Invoke(ctx);
        }

        public object Invoke(IDataContext? dataContext)
        {
            if (Exp.NodeType == ExpressionType.Call)
            {
                if (Parameters?.Length > 0)
                {
                    //if (args?.Count != Args.Length || Args.Any(arg => !args.ContainsKey(arg.Name)))
                    //{
                    //    throw new ArgumentException("miss argument");
                    //}

                    var lambda = Expression.Lambda(Exp, Parameters);
                    var args = Parameters.Select(arg =>
                    {
                        if (arg.Type == typeof(IDataContext))
                        {
                            return dataContext;
                        }
                        else if (arg.Type == typeof(FuncRefArg))
                        {
                            return new FuncRefArg(arg.Name); 
                        }

                        return (object?)null;
                    }).ToArray();

                    using (new DefaultInvokeContext())
                    {
                        return lambda.Compile().DynamicInvoke(args);
                    }

                }
            }

            return Expression.Lambda<Func<object>>(Exp).Compile()();
        }
    }
}
