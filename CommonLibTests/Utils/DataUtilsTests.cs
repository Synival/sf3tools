using CommonLib.Utils;

namespace CommonLib.Tests.Utils {
    [TestClass]
    public class DataUtilsTests {
        private static byte[] _testSubset = [0xDE, 0xAD, 0xBE, 0xEF];

        [TestMethod]
        public void IndicesOfSubset_WithMultipleMatches_ReturnsAllExpectedIndices() {
            byte[] dataSet = [
                0xDE, 0xAD, 0xBE, 0xEF,  4,  5,  6,  7,
                0xDE, 0xAD, 0xBE, 0xEF, 12, 13, 14, 15,
                0xDE, 0xAD, 0xBE, 0xEF, 20, 21, 22, 23,
                0xDE, 0xAD, 0xBE, 0xEF
            ];

            var indices = DataUtils.IndicesOfSubset(dataSet, _testSubset);
            Assert.IsTrue(Enumerable.SequenceEqual(indices, [0, 8, 16, 24]));
        }

        [TestMethod]
        public void IndicesOfSubset_WithMultipleMatches_WithAlignment_DoesntMatchUnalignedResults() {
            byte[] dataSet = [
                0xDE, 0xAD, 0xBE, 0xEF,  4,  5,  6,  7,
                0xDE, 0xAD, 0xBE, 0xEF, 12, 13, 14, 15,
                0xDE, 0xAD, 0xBE, 0xEF, 20, 21, 0xDE, 0xAD,
                0xBE, 0xEF, 26, 27
            ];

            var indices = DataUtils.IndicesOfSubset(dataSet, _testSubset, alignment: 4);
            Assert.IsTrue(Enumerable.SequenceEqual(indices, [0, 8, 16]));
        }

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

        [TestMethod]
        public void ToByteArray_WithUShortArray_ReturnsExpectedByteArray() {
            ushort[] ushortArray = [ 0xDEAD, 0xBEEF, 0xFEED, 0xDAD0 ];
            byte[] expectedByteArray = [ 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED, 0xDA, 0xD0 ];

            var byteArray = ushortArray.ToByteArray();
            Assert.IsTrue(Enumerable.SequenceEqual(byteArray, expectedByteArray));
        }

        [TestMethod]
        public void ToByteArray_WithUIntArray_ReturnsExpectedByteArray() {
            uint[] uintArray = [ 0xDEADBEEF, 0xFEEDDAD0 ];
            byte[] expectedByteArray = [ 0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED, 0xDA, 0xD0 ];

            var byteArray = uintArray.ToByteArray();
            Assert.IsTrue(Enumerable.SequenceEqual(byteArray, expectedByteArray));
        }
    }
}