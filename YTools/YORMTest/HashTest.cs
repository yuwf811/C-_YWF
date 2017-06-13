using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HashAlgorithm;

namespace YORMTest
{
    [TestClass]
    public class HashTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            IHash hash = new SHAHash();
            ulong temp = hash.Hash("123");

        }
    }
}
