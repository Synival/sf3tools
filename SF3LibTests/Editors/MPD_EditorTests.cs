using SF3.Editors.MPD;
using SF3.NamedValues;
using SF3.RawEditors;
using SF3.Types;
using static SF3.Tests.TestDataPaths;

namespace SF3.Tests.Editors {
    [TestClass]
    public class MPD_EditorTests {
        private static MPD_Editor MakeEditor() {
            var scenario = ScenarioType.Scenario1;
            var nameGetterContext = new NameGetterContext(scenario);
            var byteEditor = new ByteEditor(File.ReadAllBytes(ResourcePath(scenario, "BTL02.MPD")));
            return MPD_Editor.Create(byteEditor, nameGetterContext, scenario);
        }

        [TestMethod]
        public void ChunkEditor_WithDifferentOriginalCompressionAlgorithmForData_Recompress_UpdatesCompressedData()
        {
            // Arrange
            var mpdEditor = MakeEditor();
            var editor = new CompressedEditor(mpdEditor.CompressedEditors[5].Data);
            Assert.IsFalse(editor.NeedsRecompression);
            Assert.IsFalse(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);

            // Act
            var recompressResult = editor.Recompress();

            // Assert
            Assert.IsTrue(recompressResult);
            Assert.IsFalse(editor.NeedsRecompression);
            Assert.IsTrue(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void ChunkEditor_WithDifferentOriginalCompressionAlgorithmForData_RecompressAgain_IsModifiedIsFalse()
        {
            // Arrange
            var mpdEditor = MakeEditor();
            var editor = new CompressedEditor(mpdEditor.CompressedEditors[5].Data);
            editor.Recompress();
            editor.IsModified = false;
            Assert.IsFalse(editor.NeedsRecompression);
            Assert.IsFalse(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);

            // Act
            var recompressResult = editor.Recompress();

            // Assert
            Assert.IsTrue(recompressResult);
            Assert.IsFalse(editor.NeedsRecompression);
            Assert.IsFalse(editor.IsModified);
            Assert.IsFalse(editor.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void Create_WithoutChanges_NothingHasIsModifiedFlag()
        {
            // Arrange + Act
            var editor = MakeEditor();

            // Assert
            foreach (var ce in editor.ChildEditors)
                Assert.IsFalse(ce.IsModified);
            Assert.IsFalse(editor.Editor.IsModified);
            Assert.IsFalse(editor.IsModified);
        }

        [TestMethod]
        public void Recompress_WithoutChanges_HasExpectedIsModifiedFlags()
        {
            // Arrange
            var editor = MakeEditor();

            // Act
            var recompressResult = editor.Recompress(false);

            // Assert
            Assert.IsTrue(recompressResult);

            // NOTE: Recompressing an actual .MPD file uses a different algorithm than originally used
            //       so the content of the chunks will have been modified from the original test data.
            //       The MPD_Editor should be an a modified state.

            for (int i = 0; i < editor.Chunks.Length; i++) {
                var chunkEditor = editor.ChunkEditors[i];
                var compressedEditor = editor.CompressedEditors[i];

                if (chunkEditor != null)
                    Assert.IsFalse(chunkEditor.IsModified);
                if (compressedEditor != null) {
                    // Chunks 9 and 10 is actually unmodified, how about that!
                    var expectedResult = (i == 9 || i == 10) ? false : true;
                    Assert.IsFalse(compressedEditor.NeedsRecompression);
                    Assert.AreEqual(expectedResult, compressedEditor.IsModified);
                }
            }

            Assert.IsTrue(editor.Editor.IsModified);
            Assert.IsTrue(editor.IsModified);
        }

        [TestMethod]
        public void Recompress_WithoutChanges_ASecondTime_ResultsInExpectedState()
        {
            // Arrange
            var editor = MakeEditor();
            editor.Recompress(false);
            editor.IsModified = false;
            Assert.IsFalse(editor.Editor.IsModified);

            // Act
            var recompressResult = editor.Recompress(false);

            // Assert
            Assert.IsTrue(recompressResult);
            foreach (var ce in editor.ChildEditors)
                Assert.IsFalse(ce.IsModified);
            Assert.IsFalse(editor.Editor.IsModified);
        }

        [TestMethod]
        public void TextureChunks_DataIsModified_SetsIfModifiedFlags()
        {
            // Arrange
            var editor = MakeEditor();
            Assert.IsFalse(editor.CompressedEditors[6].IsModified);
            Assert.IsFalse(editor.ChunkEditors[6].IsModified);
            Assert.IsFalse(editor.Editor.IsModified);
            Assert.IsFalse(editor.IsModified);

            // Act
            editor.TextureChunks[0].TextureTable.Rows[0].Width *= 2;

            // Assert
            Assert.IsTrue(editor.CompressedEditors[6].IsModified);
            Assert.IsTrue(editor.ChunkEditors[6].IsModified);
            Assert.IsTrue(editor.Editor.IsModified);
            Assert.IsTrue(editor.IsModified);
        }

        [TestMethod]
        public void Finalize_WithoutChanges_NothingHasIsModifiedFlag()
        {
            // Arrange
            var editor = MakeEditor();

            // Act
            editor.Finalize();

            // Assert
            foreach (var ce in editor.ChildEditors)
                Assert.IsFalse(ce.IsModified);
            Assert.IsFalse(editor.Editor.IsModified);
        }

        [TestMethod]
        public void Finalize_WithChanges_CompressedEditorHasIsModifiedFlag()
        {
            // Arrange
            var editor = MakeEditor();
            editor.TextureChunks[0].TextureTable.Rows[0].Width *= 2;

            // Act
            editor.Finalize();

            // Assert
            for (int i = 0; i < editor.Chunks.Length; i++) {
                var chunkEditor = editor.ChunkEditors[i];
                var compressedEditor = editor.CompressedEditors[i];

                if (chunkEditor != null)
                    Assert.IsFalse(chunkEditor.IsModified);

                var expectedModifiedFlag = (i == 6);
                if (compressedEditor != null)
                    Assert.AreEqual(expectedModifiedFlag, compressedEditor.IsModified);
            }

            Assert.IsTrue(editor.Editor.IsModified);
            Assert.IsTrue(editor.IsModified);
        }

        [TestMethod]
        public void ChunkEditor_SettingIsModifiedToTrue_SetsEditorIsModifiedToTrue() {
            for (int chunkToTest = 0; chunkToTest < 32; chunkToTest++) {
                // Arrange
                var editor = MakeEditor();
                var chunkEditor = editor.ChunkEditors[chunkToTest];
                if (chunkEditor == null)
                    continue;
                Assert.IsFalse(editor.Editor.IsModified);

                // Act
                chunkEditor.IsModified = true;

                // Assert
                Assert.IsTrue(editor.Editor.IsModified);
                Assert.IsTrue(editor.IsModified);
            }
        }

        [TestMethod]
        public void Editor_IsModified_SettingToFalseWhenNoRecompressionIsRequired_IsSetToFalse()
        {
            // Arrange
            var editor = MakeEditor();
            editor.Editor.IsModified = true;
            Assert.IsFalse(editor.CompressedEditors.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(editor.ChildEditors.Any(x => x.IsModified));

            // Act
            editor.Editor.IsModified = false;

            // Assert
            Assert.IsFalse(editor.CompressedEditors.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(editor.ChildEditors.Any(x => x.IsModified));
            Assert.IsFalse(editor.Editor.IsModified);
            Assert.IsFalse(editor.IsModified);
        }

        [TestMethod]
        public void CompressedEditor_NeedsRecompressionSet_ResultsInExpectedEditorState()
        {
            // Arrange
            var editor = MakeEditor();
            editor.Editor.IsModified = true;
            Assert.IsFalse(editor.CompressedEditors.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(editor.ChildEditors.Any(x => x.IsModified));
            var compressedEditor = editor.CompressedEditors[5];

            // Act
            compressedEditor.NeedsRecompression = true;

            // Assert
            Assert.IsFalse(editor.CompressedEditors.Where(x => x != null && x != compressedEditor).Any(x => x.NeedsRecompression));
            Assert.IsFalse(editor.ChildEditors.Where(x => x != compressedEditor).Any(x => x.IsModified));

            Assert.IsTrue(compressedEditor.NeedsRecompression);
            Assert.IsTrue(compressedEditor.IsModified);

            Assert.IsTrue(editor.Editor.IsModified);
            Assert.IsTrue(editor.IsModified);
        }

        [TestMethod]
        public void Editor_IsModifiedSetToFalseWhenRecompressionIsRequired_IsStillTrue()
        {
            // Arrange
            var editor = MakeEditor();
            editor.Editor.IsModified = true;
            Assert.IsFalse(editor.CompressedEditors.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(editor.ChildEditors.Any(x => x.IsModified));
            var compressedEditor = editor.CompressedEditors[5];
            compressedEditor.NeedsRecompression = true;

            // Act
            editor.IsModified = false;

            // Assert
            Assert.IsTrue(compressedEditor.NeedsRecompression);
            Assert.IsTrue(compressedEditor.IsModified);
            Assert.IsFalse(editor.CompressedEditors.Where(x => x != null && x != compressedEditor).Any(x => x.NeedsRecompression));
            Assert.IsFalse(editor.ChildEditors.Where(x => x != compressedEditor).Any(x => x.IsModified));
            Assert.IsFalse(editor.Editor.IsModified);
            Assert.IsTrue(editor.IsModified);
        }
    }
}