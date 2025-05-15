using CommonLib.Utils;

namespace CommonLib.Tests.Utils {
    [TestClass]
    public class DataUtilsTests {
        private static byte[] _testSubset = [0xDE, 0xAD, 0xBE, 0xEF];

        [TestMethod]
        public void IndexOfSubset_WithTestSubsetInTheMiddle_ReturnsExpectedIndex() {
            byte[] dataSet = [0, 1, 2, 3, 0xDE, 0xAD, 0xBE, 0xEF, 8, 9, 10, 11];
            var index = DataUtils.IndexOfSubset(dataSet, _testSubset);
            Assert.AreEqual(4, index);
        }

        [TestMethod]
        public void IndexOfSubset_WithTestSubsetAtStart_ReturnsExpectedIndex() {
            byte[] dataSet = [0xDE, 0xAD, 0xBE, 0xEF, 4, 5, 6, 7, 8, 9, 10, 11];
            var index = DataUtils.IndexOfSubset(dataSet, _testSubset);
            Assert.AreEqual(0, index);
        }

        [TestMethod]
        public void IndexOfSubset_WithTestSubsetAtEnd_ReturnsExpectedIndex() {
            byte[] dataSet = [0, 1, 2, 3, 4, 5, 6, 7, 0xDE, 0xAD, 0xBE, 0xEF];
            var index = DataUtils.IndexOfSubset(dataSet, _testSubset);
            Assert.AreEqual(8, index);
        }

        [TestMethod]
        public void IndexOfSubset_WithNoSubset_ReturnsNegativeOne() {
            byte[] dataSet = [0, 1, 2, 3, 4, 5, 6, 7];
            var index = DataUtils.IndexOfSubset(dataSet, _testSubset);
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfSubset_WithSimilarSubsetInTheMiddle_ReturnsNegativeOne() {
            byte[] dataSet = [0, 1, 2, 3, 0xDE, 0xAD, 0xBE, 0xFF, 8, 9, 10, 11];
            var index = DataUtils.IndexOfSubset(dataSet, _testSubset);
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfSubset_WithSimilarSubsetAtStart_ReturnsNegativeOne() {
            byte[] dataSet = [0xDE, 0xAD, 0xBE, 0xFF, 4, 5, 6, 7, 8, 9, 10, 11];
            var index = DataUtils.IndexOfSubset(dataSet, _testSubset);
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfSubset_WithSimilarSubsetAtEnd_ReturnsNegativeOne() {
            byte[] dataSet = [0, 1, 2, 3, 4, 5, 6, 7, 0xDE, 0xAD, 0xBE, 0xFF];
            var index = DataUtils.IndexOfSubset(dataSet, _testSubset);
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfSubset_WithTruncatedSubsetAtStart_ReturnsNegativeOne() {
            byte[] dataSet = [0xDE, 0xAD, 0xBE, 4, 5, 6, 7, 8, 9, 10, 11];
            var index = DataUtils.IndexOfSubset(dataSet, _testSubset);
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfSubset_WithTruncatedSubsetAtEnd_ReturnsNegativeOne() {
            byte[] dataSet = [0, 1, 2, 3, 4, 5, 6, 7, 0xDE, 0xAD, 0xBE];
            var index = DataUtils.IndexOfSubset(dataSet, _testSubset);
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void IndexOfSubset_WithTruncatedSubsetInTheMiddle_ReturnsNegativeOne() {
            byte[] dataSet = [0, 1, 2, 3, 0xDE, 0xAD, 0xBE, 7, 8, 9, 10, 11];
            var index = DataUtils.IndexOfSubset(dataSet, _testSubset);
            Assert.AreEqual(-1, index);
        }
    }
}