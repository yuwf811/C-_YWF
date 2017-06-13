using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YOrm.Attributes;
using YOrm.Infrastructure;

namespace YOrm
{
    public class YDb
    {
        string _connStr;
        public YDb(string connStr)
        {
            this._connStr = connStr;
        }

        public DataTable Get<T>(string condition)
        {
            Type type = typeof(T);
            string query = string.Empty;
            if (string.IsNullOrEmpty(condition))
            {
                query = string.Format("Select {0} From [{1}]", type.GetProperties().Select(p => p.Name).ToStringWithChar(','), type.Name);
            }
            else
            {
                query = string.Format("Select {0} From [{1}] Where {2}", type.GetProperties().Select(p => p.Name).ToStringWithChar(','), type.Name, condition);
            }
            using (SqlConnection sqlConn = new SqlConnection(_connStr))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                sda.Fill(table);
                if (table == null)
                    return null;
                return table;
            }
        }

        public void Add<T>(T entity)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties().Where(p=>p.GetCustomAttributes(typeof(YIdentityAttribute)).Count()==0).ToArray();

            string query = string.Format("Insert Into [{0}]({1}) Values ({2})", type.Name, properties.Select(p => p.Name).ToStringWithChar(','),
                properties.Select(p => "@" + p.Name).ToStringWithChar(','));
            SqlParameter[] parameters = new SqlParameter[properties.Length];
            using (SqlConnection sqlConn = new SqlConnection(_connStr))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                foreach (var property in type.GetProperties())
                {
                    cmd.Parameters.Add(new SqlParameter("@" + property.Name, property.GetValue(entity)));
                }
                cmd.ExecuteNonQuery();
            }
        }

        public void Update<T>(T entity)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            PropertyInfo[] keys = properties.Where(p => p.GetCustomAttributes(typeof(YKeyAttribute)).Count() > 0).ToArray();
            PropertyInfo[] others = properties.Except(keys).Where(p => p.GetCustomAttributes(typeof(YIdentityAttribute)).Count() == 0).ToArray();

            string query = string.Format("Update [{0}] Set {1} Where {2}", type.Name, others.Select(o => o.Name + "=@" + o.Name).ToStringWithChar(','),
                keys.Select(o => o.Name + "=@" + o.Name).ToStringWithChar(','));
            using (SqlConnection sqlConn = new SqlConnection(_connStr))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                foreach (var property in others)
                {
                    cmd.Parameters.Add(new SqlParameter("@" + property.Name, property.GetValue(entity)));
                }
                foreach (var property in keys)
                {
                    cmd.Parameters.Add(new SqlParameter("@" + property.Name, property.GetValue(entity)));
                }
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete<T>(T entity)
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            PropertyInfo key = properties.First(p => p.GetCustomAttributes(typeof(YKeyAttribute)).Count() > 0);
            string query = string.Format("Delete [{0}] Where {1}={2}", type.Name, "@" + key.Name, key.Name);
            using (SqlConnection sqlConn = new SqlConnection(_connStr))
            {
                sqlConn.Open();
                SqlCommand cmd = new SqlCommand(query, sqlConn);
                cmd.Parameters.Add(new SqlParameter("@" + key.Name, key.GetValue(entity)));
                cmd.ExecuteNonQuery();
            }
        }
    }
}
