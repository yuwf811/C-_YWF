using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YOrm.Infrastructure
{
    public static class Extensions
    {
        public static string ToStringWithChar(this IEnumerable<string> list, char chararctor)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append(item + chararctor);
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public static IEnumerable<T> ConvertToEntity<T>(this DataTable table)
        {
            foreach (DataRow row in table.Rows)
            {
                T t = Activator.CreateInstance<T>();
                Type type = typeof(T);
                foreach (var property in type.GetProperties())
                {
                    if (table.Columns.Contains(property.Name))
                    {
                        property.SetValue(t, row[property.Name]);
                    }
                }

                yield return t;
            }
        }

        public static string GetQueryString<T>(T value)
        {
            StringBuilder sb = new StringBuilder();
            Type t = typeof(T);
            foreach (var property in t.GetProperties())
            {
                Type type = property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) ?
                    property.PropertyType.GetGenericArguments()[0] : property.PropertyType;

                if (type == typeof(DateTime))
                {
                    sb.Append("CONVERT(varchar(100), " + property.Name + ", 20)" + "='" + ((DateTime)property.GetValue(value)).ToString("yyyy-MM-dd HH:mm:ss") + "' And ");
                }
                else
                {
                    sb.Append(property.Name + "='" + property.GetValue(value).ToString() + "' And ");
                }
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 5, 5);
            }

            return sb.ToString();
        }
    }
}
