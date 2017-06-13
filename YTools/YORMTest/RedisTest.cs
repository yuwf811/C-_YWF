using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedisTool;

namespace YORMTest
{
    [TestClass]
    public class RedisTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            RedisHelper helper = new RedisHelper("127.0.0.1",6379);

            long index = helper.ZRank("ZSET", "");


            string[] array = helper.ZRange("ZSET", 0,1);
        }
    }
}
