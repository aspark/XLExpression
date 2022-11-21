using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using XLExpression.Common;
using XLExpression.Functions;
using XLExpression.Nodes;
using XLParser;

[assembly: InternalsVisibleToAttribute("XLExpression.Test")]

namespace XLExpression
{
    public class ExpressionBuilder
    {
        internal ExpressionBuilder()
        {

        }

        /// <summary>
        /// 将公式转为表达式树
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public ExpressionResult Build(string formula)
        {
            var parameters = new Dictionary<string, ParameterExpression>(StringComparer.InvariantCultureIgnoreCase);

            var result = new ExpressionResult(Build(ConvertToNode(formula), ref parameters)) { Parameters = parameters.Values.ToArray() };

            return result;
        }

        /// <summary>
        /// 将公式转为代码
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public string BuildToCode(string formula)
        {
            var names = new Dictionary<string, ParameterExpression>(StringComparer.InvariantCultureIgnoreCase);

            return Build(ConvertToNode(formula), ref names)?.ToString() ?? "";
        }

        private Expression? Build(ExpressionNode? node, ref Dictionary<string, ParameterExpression> parameters)
        {
            if (node != null)
            {
                if (node.Type == NodeType.Function)
                {
                    var fnNode = (FunctionNode)node;
                    var arguments = new List<Expression?>();
                    foreach(var arg in fnNode.Arguments)
                    {
                        arguments.Add(Build(arg, ref parameters));
                    }

                    var fcInstance = Expression.Property(null, typeof(FunctionFactory).GetProperty(nameof(FunctionFactory.Instance)));
                    var fnOperator = Expression.Call(fcInstance, typeof(FunctionFactory).GetMethod(nameof(FunctionFactory.GetOperator)), Expression.Constant(fnNode.Name));//Factory.Instance.GetOperator("").Invoke(object[])
                    var dataContext = parameters.GetOrAdd("dataContext", () => Expression.Parameter(typeof(IFunctionDataContext), "dataContext"));

                    return Expression.Call(fnOperator, typeof(IFunction).GetMethod(nameof(IFunction.Invoke)), dataContext, Expression.NewArrayInit(typeof(object), arguments.ToArray()));
                }
                else if (node.Type == NodeType.Const)
                {
                    return Expression.Constant((node as ConstNode)!.Value, typeof(object));
                }
                else if (node.Type == NodeType.Ref)
                {
                    //按参数返回
                    var refNode = node as RefNode;

                    var paraName = refNode!.Name;
                    ParameterExpression? parameter = parameters.GetOrAdd(paraName, () => Expression.Parameter(typeof(FuncRefArg), paraName));

                    return parameter;
                }
            }

            return null;
        }

        public static ExpressionBuilder Instance => new ExpressionBuilder();


        internal ExpressionNode? ConvertToNode(string formula)
        {
            if (formula != null)
            {
                var xlNode = ExcelFormulaParser.Parse(formula.Trim('='));

                //只有公式
                if(xlNode.Term?.Name?.Equals("formula", StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    return ConvertToNode(xlNode);
                }
                else
                {
                    throw new NotImplementedException("not support" + formula);
                }
            }

            return null;
        }

        /// <summary>
        /// 转换为方法调用的简单结构
        /// </summary>
        /// <param name="xlNode"></param>
        /// <returns></returns>
        private ExpressionNode? ConvertToNode(ParseTreeNode xlNode)
        {
            xlNode = GetRealParentNode(xlNode);

            if (xlNode.IsFunction())
            {
                var node = new FunctionNode() { Name = xlNode.GetFunction() };
                foreach (var xlArg in xlNode.GetFunctionArguments())
                {
                    node.Arguments.Add(ConvertToNode(xlArg));
                }

                return node;
            }
            else if (xlNode.Token != null)
            {
                if(xlNode.Term.Name.Equals("CellToken", StringComparison.InvariantCultureIgnoreCase))
                {
                    //参数
                    return new RefNode() { Name = xlNode.Token.Text };
                }
                
                return new ConstNode() { Value = xlNode.Token.Value };
            }

            return null;
        }

        private ParseTreeNode GetRealParentNode(ParseTreeNode node)
        {
            ParseTreeNode target = node;
            while (target?.ChildNodes?.Count == 1)
            {
                target = target.ChildNodes[0];
            }

            return target;
        }
    }
}
