using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib.Extensions;

namespace CommonLib.Tests.Extensions {
    [TestClass]
    public class ArrayExtensions {
        [TestMethod]
        public void To2DArray_OrdersDataProperly()
        {
            int[] inputArray = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12};
            var resultArray = inputArray.To2DArray(3, 4);

            Assert.AreEqual( 1, resultArray[0, 0]);
            Assert.AreEqual( 2, resultArray[0, 1]);
            Assert.AreEqual( 3, resultArray[0, 2]);
            Assert.AreEqual( 4, resultArray[0, 3]);
            Assert.AreEqual( 5, resultArray[1, 0]);
            Assert.AreEqual( 6, resultArray[1, 1]);
            Assert.AreEqual( 7, resultArray[1, 2]);
            Assert.AreEqual( 8, resultArray[1, 3]);
            Assert.AreEqual( 9, resultArray[2, 0]);
            Assert.AreEqual(10, resultArray[2, 1]);
            Assert.AreEqual(11, resultArray[2, 2]);
            Assert.AreEqual(12, resultArray[2, 3]);
        }

        [TestMethod]
        public void To1DArray_OrdersDataProperly()
        {
            int[,] inputArray = {{1, 2, 3, 4}, {5, 6, 7, 8}, {9, 10, 11, 12}};
            var resultArray = inputArray.To1DArray();

            Assert.AreEqual( 1, resultArray[0]);
            Assert.AreEqual( 2, resultArray[1]);
            Assert.AreEqual( 3, resultArray[2]);
            Assert.AreEqual( 4, resultArray[3]);
            Assert.AreEqual( 5, resultArray[4]);
            Assert.AreEqual( 6, resultArray[5]);
            Assert.AreEqual( 7, resultArray[6]);
            Assert.AreEqual( 8, resultArray[7]);
            Assert.AreEqual( 9, resultArray[8]);
            Assert.AreEqual(10, resultArray[9]);
            Assert.AreEqual(11, resultArray[10]);
            Assert.AreEqual(12, resultArray[11]);
        }
    }
}
