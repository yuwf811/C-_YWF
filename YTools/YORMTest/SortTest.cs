using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SortAlgorithm;
using MSSQLTool;

namespace YORMTest
{
    [TestClass]
    public class SortTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Guid newGuid = GuidGenerator.NewComb();
            DateTime now = GuidGenerator.GetDateFromComb(newGuid);

            ISort sort = new InsertSort();

            int[] array = { 2, 3, 1, 5, 2, 3, 6, 4, 9, 2 };

            sort.Sort<int>(array);


        }
    }
}
