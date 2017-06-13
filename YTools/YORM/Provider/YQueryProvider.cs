using Autofac;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YOrm.Infrastructure;
using YOrm.Interpreter;

namespace YOrm.Provider
{
    public class YQueryProvider<T> : IDbQueryProvider
    {
        private IContainer _container;
        private string _connStr;
        private YTable<T> _table;

        public YQueryProvider(YTable<T> table)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<YInterpreter>().As<IInterpreter>();
            _container = builder.Build();
            _table = table;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            _table.SetExpression(expression);
            return (IQueryable<TElement>)_table;
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }

        public TResult Execute<TResult>(Expression expression)
        {
            YDb ydb = new YDb(_connStr);
            if (expression.NodeType == ExpressionType.Call)
            {
                MethodCallExpression methodCall = expression as MethodCallExpression;
                IInterpreter interpreter = _container.Resolve<IInterpreter>();
                string condition = methodCall.Method.Name == "Contains" ? Extensions.GetQueryString((T)((ConstantExpression)methodCall.Arguments[1]).Value)
                    : interpreter.Interpret(expression);
                DataTable dt = ydb.Get<T>(condition);

                switch (methodCall.Method.Name)
                {
                    case "Where":
                    case "First":
                    case "FirstOrDefault":
                        Type t = typeof(TResult).IsGenericType ? typeof(TResult).GetGenericArguments()[0] : typeof(TResult);
                        MethodInfo mi = typeof(Extensions).GetMethod("ConvertToEntity", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(t);
                        _table.QueryItems.Clear();

                        if (methodCall.Method.Name == "Where")
                        {
                            IEnumerable<T> result = ((IEnumerable<T>)mi.Invoke(null, new object[] { dt })).ToList();
                            foreach(var item in result)
                            {
                                _table.InitData(item);
                            }
                            return (TResult)result;
                        }
                        else if (methodCall.Method.Name == "First")
                        {
                            var result = ((IEnumerable<TResult>)mi.Invoke(null, new object[] { dt })).First();
                            _table.InitData((T)(object)result);
                            return result;
                        }
                        else
                        {
                            var result = ((IEnumerable<TResult>)mi.Invoke(null, new object[] { dt })).FirstOrDefault();
                            _table.InitData((T)(object)result);
                            return result;
                        }
                    case "Contains":
                        return (TResult)(object)(dt.Rows.Count > 0);
                    case "Count":
                        return (TResult)(object)(dt.Rows.Count);
                    case "LongCount":
                        return (TResult)(object)(dt.Rows.Count);
                    case "Any":
                        return (TResult)(object)(dt.Rows.Count > 0);
                    default:
                        return default(TResult);
                }
            }
            else if (expression.NodeType == ExpressionType.Constant)
            {
                DataTable dt = ydb.Get<T>("");
                Type t = typeof(TResult).IsGenericType ? typeof(TResult).GetGenericArguments()[0] : typeof(TResult);
                MethodInfo mi = typeof(Extensions).GetMethod("ConvertToEntity", BindingFlags.Static | BindingFlags.Public).MakeGenericMethod(t);
                return (TResult)mi.Invoke(null, new object[] { dt });
            }
            else
            {
                return default(TResult);
            }
        }

        public void SetConnection(string connStr)
        {
            _connStr = connStr;
        }
    }
}
