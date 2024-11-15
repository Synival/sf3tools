using System.Text;
using SF3.RawEditors;
using static CommonLib.Utils.Compression;

namespace SF3.Tests.RawEditors {
    [TestClass]
    public class CompressedEditorTests {
        private static readonly byte[] _compressedTestData = Compress(Encoding.UTF8.GetBytes("Hello, world!!"));

        [TestMethod]
        public void Constructor_ResultsInExpectedState()
        {
            // Arrange + Act
            var editor = new CompressedEditor(_compressedTestData);

            // Assert
            Assert.IsFalse(editor.NeedsRecompression);
            Assert.IsFalse(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void Recompress_WithoutDecompressedEditorChanges_ResultsInExpectedState()
        {
            // Arrange
            var editor = new CompressedEditor(_compressedTestData);

            // Act
            var recompressResult = editor.Recompress();

            // Assert
            Assert.IsTrue(recompressResult);
            Assert.IsFalse(editor.NeedsRecompression);
            Assert.IsFalse(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void DecompressedEditor_IsModifiedIsSet_ResultsInExpectedState()
        {
            // Arrange
            var editor = new CompressedEditor(_compressedTestData);

            // Act
            editor.DecompressedEditor.IsModified = true;

            // Assert
            Assert.IsTrue(editor.NeedsRecompression);
            Assert.IsTrue(editor.IsModified);
            Assert.IsTrue(editor.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void DecompressedEditor_IsModifiedIsToggled_ResultsInExpectedState()
        {
            // Arrange
            var editor = new CompressedEditor(_compressedTestData);

            // Act
            editor.DecompressedEditor.IsModified = true;
            editor.DecompressedEditor.IsModified = false;

            // Assert
            Assert.IsTrue(editor.NeedsRecompression);
            Assert.IsTrue(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void Recompress_AfterModificationsInDecompressedEditor_ResultsInExpectedState()
        {
            // Arrange
            var editor = new CompressedEditor(_compressedTestData);
            editor.DecompressedEditor.IsModified = true;

            // Act
            var recompressResult = editor.Recompress();

            // Assert
            Assert.IsTrue(recompressResult);
            Assert.IsFalse(editor.NeedsRecompression);
            Assert.IsTrue(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void NeedsRecompression_SetToTrue_SetsItselfAndIsModifiedToTrue()
        {
            // Arrange
            var editor = new CompressedEditor(_compressedTestData);

            // Act
            editor.NeedsRecompression = true;

            // Assert
            Assert.IsTrue(editor.NeedsRecompression);
            Assert.IsTrue(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void NeedsRecompression_ToggledOnAndOff_StillHasIsModifiedFlagSet()
        {
            // Arrange
            var editor = new CompressedEditor(_compressedTestData);

            // Act
            editor.NeedsRecompression = true;
            editor.NeedsRecompression = false;

            // Assert
            Assert.IsFalse(editor.NeedsRecompression);
            Assert.IsTrue(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void IsModified_SetToTrue_ResultsInTrue()
        {
            // Arrange
            var editor = new CompressedEditor(_compressedTestData);

            // Act
            editor.IsModified = true;

            // Assert
            Assert.IsTrue(editor.IsModified);
            Assert.IsFalse(editor.NeedsRecompression);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void IsModified_SetWhenNeedsRecompressionIsTrue_IsModifiedIsStillTrue()
        {
            // Arrange
            var editor = new CompressedEditor(_compressedTestData);
            editor.NeedsRecompression = true;

            // Act
            editor.IsModified = false;

            // Assert
            Assert.IsTrue(editor.NeedsRecompression);
            Assert.IsTrue(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);
        }
    }
}
