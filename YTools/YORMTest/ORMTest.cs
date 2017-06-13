using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YOrm;
using System.Linq;
using YOrm.Infrastructure;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using YOrm.Attributes;
using YOrm.Hash;

namespace YORMTest
{
    [TestClass]
    public class ORMTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            YWFContext context = new YWFContext("YWF");
            var newUser = new User { Id = 100, Name = "ywf", UpdateTime = DateTime.Now };
            context.User.Add(newUser);
            var users = context.User.Where(u => u.Name == "ywf");
            foreach (var user in users)
            {
                user.Name = "bbbbbb";
            }
            context.SaveChanges();
            context.SaveChanges();
            //User user2 = context.User.Where(u => u.Name == "").FirstOrDefault();
            //var users = context.User.Contains(user);
        }
    }

    public class YWFContext : YContext
    {
        public YWFContext(string connName)
            : base(connName)
        {
        }

        public YTable<User> User { get; set; }
    }

    public class User
    {
        [YKey]
        [YIdentity]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? UpdateTime { get; set; }
    }


}
