using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YOrm.Interpreter
{
    public class YInterpreter : IInterpreter
    {
        public string Interpret(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Call:
                    MethodCallExpression methodCall = expression as MethodCallExpression;
                    switch (methodCall.Method.Name)
                    {
                        case "Contains":
                        case "Count":
                        case "LongCount":
                        case "Where":
                        case "Any":
                            return methodCall.Arguments[0] is ConstantExpression ? "(" + Interpret(methodCall.Arguments[1]) + ")" :
                                "(" + Interpret(methodCall.Arguments[0]) + ")" + " And " + "(" + Interpret(methodCall.Arguments[1]) + ")";
                        case "First":
                        case "FirstOrDefault":
                            return Interpret(methodCall.Arguments[0]);
                        default:
                            throw new Exception(string.Format("不支持{0}方法的查找！", methodCall.Method.Name));
                    }
                case ExpressionType.Quote:
                    UnaryExpression unary = expression as UnaryExpression;
                    return Interpret(unary.Operand);
                case ExpressionType.Lambda:
                    LambdaExpression lambda = expression as LambdaExpression;
                    return Interpret(lambda.Body);
                case ExpressionType.Constant:
                    ConstantExpression constant = expression as ConstantExpression;
                    return constant.Value.GetType().IsValueType ? constant.Value.ToString() : "'" + constant.Value.ToString() + "'";
                case ExpressionType.Parameter:
                    ParameterExpression parameter = expression as ParameterExpression;
                    return string.Empty;
                case ExpressionType.MemberAccess:
                    MemberExpression member = expression as MemberExpression;
                    return member.Member.Name;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    BinaryExpression or = expression as BinaryExpression;
                    return "(" + Interpret(or.Left) + ") Or (" + Interpret(or.Right) + ")";
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    BinaryExpression and = expression as BinaryExpression;
                    return Interpret(and.Left) + " And " + Interpret(and.Right);
                case ExpressionType.Equal:
                    BinaryExpression equal = expression as BinaryExpression;
                    return Interpret(equal.Left) + "=" + Interpret(equal.Right);
                case ExpressionType.LessThan:
                    BinaryExpression lessThan = expression as BinaryExpression;
                    return Interpret(lessThan.Left) + "<" + Interpret(lessThan.Right);
                case ExpressionType.LessThanOrEqual:
                    BinaryExpression lessThanOrEqual = expression as BinaryExpression;
                    return Interpret(lessThanOrEqual.Left) + "<=" + Interpret(lessThanOrEqual.Right);
                case ExpressionType.GreaterThan:
                    BinaryExpression greaterThan = expression as BinaryExpression;
                    return Interpret(greaterThan.Left) + ">" + Interpret(greaterThan.Right);
                case ExpressionType.GreaterThanOrEqual:
                    BinaryExpression greaterThanOrEqual = expression as BinaryExpression;
                    return Interpret(greaterThanOrEqual.Left) + ">=" + Interpret(greaterThanOrEqual.Right);
                default:
                    return string.Empty;
            }
        }
    }
}
