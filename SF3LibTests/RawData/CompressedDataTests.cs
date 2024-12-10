using System.Text;
using CommonLib.Arrays;
using SF3.RawData;
using static CommonLib.Utils.Compression;

namespace SF3.Tests.RawData {
    [TestClass]
    public class CompressedDataTests {
        private static readonly byte[] _compressedTestData = Compress(Encoding.UTF8.GetBytes("Hello, world!!"));

        [TestMethod]
        public void Constructor_ResultsInExpectedState()
        {
            // Arrange + Act
            var data = new CompressedData(new ByteArray(_compressedTestData));

            // Assert
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsFalse(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);
        }

        [TestMethod]
        public void Recompress_WithoutDeCompressedDataChanges_ResultsInExpectedState()
        {
            // Arrange
            var data = new CompressedData(new ByteArray(_compressedTestData));

            // Act
            var recompressResult = data.Recompress();

            // Assert
            Assert.IsTrue(recompressResult);
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsFalse(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);
        }

        [TestMethod]
        public void DeCompressedData_IsModifiedIsSet_ResultsInExpectedState()
        {
            // Arrange
            var data = new CompressedData(new ByteArray(_compressedTestData));

            // Act
            data.DecompressedData.IsModified = true;

            // Assert
            Assert.IsTrue(data.NeedsRecompression);
            Assert.IsTrue(data.IsModified);
            Assert.IsTrue(data.DecompressedData.IsModified);
        }

        [TestMethod]
        public void DeCompressedData_IsModifiedIsToggled_ResultsInExpectedState()
        {
            // Arrange
            var data = new CompressedData(new ByteArray(_compressedTestData));

            // Act
            data.DecompressedData.IsModified = true;
            data.DecompressedData.IsModified = false;

            // Assert
            Assert.IsTrue(data.NeedsRecompression);
            Assert.IsTrue(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);
        }

        [TestMethod]
        public void Recompress_AfterModificationsInDeCompressedData_ResultsInExpectedState()
        {
            // Arrange
            var data = new CompressedData(new ByteArray(_compressedTestData));
            data.DecompressedData.IsModified = true;

            // Act
            var recompressResult = data.Recompress();

            // Assert
            Assert.IsTrue(recompressResult);
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsTrue(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);
        }

        [TestMethod]
        public void NeedsRecompression_SetToTrue_SetsItselfAndIsModifiedToTrue()
        {
            // Arrange
            var data = new CompressedData(new ByteArray(_compressedTestData));

            // Act
            data.NeedsRecompression = true;

            // Assert
            Assert.IsTrue(data.NeedsRecompression);
            Assert.IsTrue(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);
        }

        [TestMethod]
        public void NeedsRecompression_ToggledOnAndOff_StillHasIsModifiedFlagSet()
        {
            // Arrange
            var data = new CompressedData(new ByteArray(_compressedTestData));

            // Act
            data.NeedsRecompression = true;
            data.NeedsRecompression = false;

            // Assert
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsTrue(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);
        }

        [TestMethod]
        public void IsModified_SetToTrue_ResultsInTrue()
        {
            // Arrange
            var data = new CompressedData(new ByteArray(_compressedTestData));

            // Act
            data.IsModified = true;

            // Assert
            Assert.IsTrue(data.IsModified);
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsFalse(data.DecompressedData.IsModified);
        }

        [TestMethod]
        public void IsModified_SetWhenNeedsRecompressionIsTrue_IsModifiedIsStillTrue()
        {
            // Arrange
            var data = new CompressedData(new ByteArray(_compressedTestData));
            data.NeedsRecompression = true;

            // Act
            data.IsModified = false;

            // Assert
            Assert.IsTrue(data.NeedsRecompression);
            Assert.IsTrue(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);
        }
    }
}
