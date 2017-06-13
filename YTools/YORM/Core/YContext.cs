using Autofac;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YOrm.Attributes;
using YOrm.Hash;

namespace YOrm
{
    public class YContext
    {
        private YContextConfigurations _config;
        private IContainer _container;

        public YContext(string connName)
        {
            if (string.IsNullOrEmpty(ConfigurationManager.ConnectionStrings[connName].ToString()))
            {
                throw (new Exception());
            }
            _config = new YContextConfigurations { DBConnectionStr = ConfigurationManager.ConnectionStrings[connName].ToString() };
            var builder = new ContainerBuilder();
            builder.RegisterType<MD5Computer>().As<IHashComputer>();
            _container = builder.Build();
            InitialSets();
        }

        private void InitialSets()
        {
            Type type = GetType();
            foreach (var property in type.GetProperties())
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition()==typeof(YTable<>))
                {
                    Type propertyType = property.PropertyType;
                    property.SetValue(this, Activator.CreateInstance(propertyType));
                    Type yType = property.PropertyType;

                    if (yType.GetGenericArguments()[0].GetProperties().Any(p => p.GetCustomAttributes(typeof(YKeyAttribute)).Count() > 0))
                    {
                        yType.GetMethod("SetConnection").Invoke(property.GetValue(this, null), new[] { _config.DBConnectionStr });
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public void SaveChanges()
        {
            Type type = GetType();
            
            //保存每个YTable
            foreach (PropertyInfo property in type.GetProperties().Where(p=> p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(YTable<>)))
            {
                Type innerType = property.PropertyType.GetGenericArguments()[0];
                var propertyValue = property.GetValue(this);
                var queryItems = property.PropertyType.GetProperty("QueryItems").GetValue(propertyValue);
                var addedItems = property.PropertyType.GetProperty("AddedItems").GetValue(propertyValue);
                var deletedItems = property.PropertyType.GetProperty("DeletedItems").GetValue(propertyValue);
                var originalHashSet = property.PropertyType.GetProperty("OriginalHashSet").GetValue(propertyValue) as HashSet<string>;
                MethodInfo md5 = typeof(MD5Computer).GetMethod("ComputeHash").MakeGenericMethod(innerType);
                MethodInfo addToDb = typeof(YDb).GetMethod("Add").MakeGenericMethod(innerType);
                MethodInfo updateDb = typeof(YDb).GetMethod("Update").MakeGenericMethod(innerType);
                MethodInfo deleteFromDb = typeof(YDb).GetMethod("Delete").MakeGenericMethod(innerType);
                MethodInfo clearItems = property.PropertyType.GetMethod("ClearItems");
                YDb ydb = new YDb(_config.DBConnectionStr);
                foreach (var item in (IEnumerable)queryItems)
                {
                    if (!originalHashSet.Contains(md5.Invoke(new MD5Computer(), new object[] { item })))
                    {
                        updateDb.Invoke(ydb, new object[] { item });
                    }
                }

                foreach (var item in (IEnumerable)addedItems)
                {
                    addToDb.Invoke(ydb, new object[] { item });
                }

                foreach (var item in (IEnumerable)deletedItems)
                {
                    deleteFromDb.Invoke(ydb, new object[] { item });
                }

                clearItems.Invoke(propertyValue, null);
            }
        }
    }
}
