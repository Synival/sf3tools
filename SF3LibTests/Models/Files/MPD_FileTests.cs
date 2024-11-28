using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.RawData;
using SF3.Types;
using static SF3.Tests.TestDataPaths;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class MPD_FileTests {
        private static MPD_File MakeFile() {
            var scenario = ScenarioType.Scenario1;
            var nameGetterContext = new NameGetterContext(scenario);
            var byteEditor = new ByteData(File.ReadAllBytes(ResourcePath(scenario, "BTL02.MPD")));
            return MPD_File.Create(byteEditor, nameGetterContext, scenario);
        }

        [TestMethod]
        public void ChunkEditor_WithDifferentOriginalCompressionAlgorithmForData_Recompress_UpdatesCompressedData() {
            // Arrange
            var mpdFile = MakeFile();
            var data = new CompressedData(mpdFile.ChunkEditors[5].Data);
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsFalse(data.IsModified);
            Assert.IsFalse(data.DecompressedEditor.IsModified);

            // Act
            var recompressResult = data.Recompress();

            // Assert
            Assert.IsTrue(recompressResult);
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsTrue(data.IsModified);
            Assert.IsFalse(data.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void ChunkEditor_WithDifferentOriginalCompressionAlgorithmForData_RecompressAgain_IsModifiedIsFalse() {
            // Arrange
            var mpdFile = MakeFile();
            var data = new CompressedData(mpdFile.ChunkEditors[5].Data);
            data.Recompress();
            data.IsModified = false;
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsFalse(data.IsModified);
            Assert.IsFalse(data.DecompressedEditor.IsModified);

            // Act
            var recompressResult = data.Recompress();

            // Assert
            Assert.IsTrue(recompressResult);
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsFalse(data.IsModified);
            Assert.IsFalse(data.DecompressedEditor.IsModified);
        }

        [TestMethod]
        public void Create_WithoutChanges_NothingHasIsModifiedFlag() {
            // Arrange + Act
            var data = MakeFile();

            // Assert
            foreach (var ce in data.ChunkEditors.Where(x => x != null))
                Assert.IsFalse(ce.IsModified);
            Assert.IsFalse(data.Data.IsModified);
            Assert.IsFalse(data.IsModified);
        }

        [TestMethod]
        public void Recompress_WithoutChanges_HasExpectedIsModifiedFlags() {
            // Arrange
            var data = MakeFile();

            // Act
            var recompressResult = data.Recompress(false);

            // Assert
            Assert.IsTrue(recompressResult);

            for (var i = 0; i < data.Chunks.Length; i++) {
                var chunkEditor = data.ChunkEditors[i];
                if (chunkEditor == null)
                    continue;

                if (!chunkEditor.IsCompressed)
                    Assert.IsFalse(chunkEditor.IsModified);
                else {
                    // NOTE: Recompressing an actual .MPD file uses a different algorithm than originally used
                    //       so the content of the chunks will have been modified from the original test data.
                    //       The MPD_Editor should be an a modified state.
                    //       Chunks 9 and 10 are actually unmodified, how about that!
                    var expectedResult = i == 9 || i == 10 ? false : true;
                    Assert.IsFalse(chunkEditor.NeedsRecompression);
                    Assert.IsFalse(chunkEditor.DecompressedEditor.IsModified);
                    Assert.AreEqual(expectedResult, chunkEditor.IsModified);
                }
            }

            Assert.IsTrue(data.Data.IsModified);
            Assert.IsTrue(data.IsModified);
        }

        [TestMethod]
        public void Recompress_WithoutChanges_ASecondTime_ResultsInExpectedState() {
            // Arrange
            var data = MakeFile();
            data.Recompress(false);
            data.IsModified = false;
            Assert.IsFalse(data.Data.IsModified);

            // Act
            var recompressResult = data.Recompress(false);

            // Assert
            Assert.IsTrue(recompressResult);
            foreach (var ce in data.ChunkEditors.Where(x => x != null)) {
                Assert.IsFalse(ce.IsModified);
                Assert.IsFalse(ce.DecompressedEditor.IsModified);
            }
            Assert.IsFalse(data.Data.IsModified);
        }

        [TestMethod]
        public void TextureChunks_DataIsModified_SetsIfModifiedFlags() {
            // Arrange
            var data = MakeFile();
            Assert.IsFalse(data.ChunkEditors[6].IsModified);
            Assert.IsFalse(data.ChunkEditors[6].DecompressedEditor.IsModified);
            Assert.IsFalse(data.Data.IsModified);
            Assert.IsFalse(data.IsModified);

            // Act
            data.TextureChunks[0].TextureTable.Rows[0].Width *= 2;

            // Assert
            Assert.IsTrue(data.ChunkEditors[6].IsModified);
            Assert.IsTrue(data.ChunkEditors[6].DecompressedEditor.IsModified);
            Assert.IsTrue(data.Data.IsModified);
            Assert.IsTrue(data.IsModified);
        }

        [TestMethod]
        public void Finalize_WithoutChanges_NothingHasIsModifiedFlag() {
            // Arrange
            var data = MakeFile();

            // Act
            data.Finalize();

            // Assert
            foreach (var ce in data.ChunkEditors.Where(x => x != null)) {
                Assert.IsFalse(ce.IsModified);
                Assert.IsFalse(ce.DecompressedEditor.IsModified);
            }
            Assert.IsFalse(data.Data.IsModified);
        }

        [TestMethod]
        public void Finalize_WithChanges_CompressedEditorHasIsModifiedFlag() {
            // Arrange
            var data = MakeFile();
            data.TextureChunks[0].TextureTable.Rows[0].Width *= 2;

            // Act
            data.Finalize();

            // Assert
            for (var i = 0; i < data.Chunks.Length; i++) {
                var chunkEditor = data.ChunkEditors[i];
                if (chunkEditor == null)
                    continue;

                Assert.IsFalse(chunkEditor.DecompressedEditor.IsModified);
                var expectedModifiedFlag = i == 6;
                Assert.AreEqual(expectedModifiedFlag, chunkEditor.IsModified);
            }

            Assert.IsTrue(data.Data.IsModified);
            Assert.IsTrue(data.IsModified);
        }

        [TestMethod]
        public void ChunkEditor_SettingIsModifiedToTrue_SetsEditorIsModifiedToTrue() {
            for (var chunkToTest = 0; chunkToTest < 32; chunkToTest++) {
                // Arrange
                var data = MakeFile();
                var chunkEditor = data.ChunkEditors[chunkToTest];
                if (chunkEditor == null)
                    continue;
                Assert.IsFalse(data.Data.IsModified);

                // Act
                chunkEditor.IsModified = true;

                // Assert
                Assert.IsTrue(data.Data.IsModified);
                Assert.IsTrue(data.IsModified);
            }
        }

        [TestMethod]
        public void Editor_IsModified_SettingToFalseWhenNoRecompressionIsRequired_IsSetToFalse() {
            // Arrange
            var data = MakeFile();
            data.Data.IsModified = true;
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null).Any(x => x.IsModified));

            // Act
            data.Data.IsModified = false;

            // Assert
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null).Any(x => x.IsModified));
            Assert.IsFalse(data.Data.IsModified);
            Assert.IsFalse(data.IsModified);
        }

        [TestMethod]
        public void CompressedEditor_NeedsRecompressionSet_ResultsInExpectedEditorState() {
            // Arrange
            var data = MakeFile();
            data.Data.IsModified = true;
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null).Any(x => x.IsModified));
            var chunkEditor = data.ChunkEditors[5];

            // Act
            chunkEditor.NeedsRecompression = true;

            // Assert
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null && x != chunkEditor).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null && x != chunkEditor).Any(x => x.IsModified));

            Assert.IsTrue(chunkEditor.NeedsRecompression);
            Assert.IsTrue(chunkEditor.IsModified);

            Assert.IsTrue(data.Data.IsModified);
            Assert.IsTrue(data.IsModified);
        }

        [TestMethod]
        public void Editor_IsModifiedSetToFalseWhenRecompressionIsRequired_IsStillTrue() {
            // Arrange
            var data = MakeFile();
            data.Data.IsModified = true;
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null).Any(x => x.IsModified));
            var chunkEditor = data.ChunkEditors[5];
            chunkEditor.NeedsRecompression = true;

            // Act
            data.IsModified = false;

            // Assert
            Assert.IsTrue(chunkEditor.NeedsRecompression);
            Assert.IsTrue(chunkEditor.IsModified);
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null && x != chunkEditor).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkEditors.Where(x => x != null && x != chunkEditor).Any(x => x.IsModified));
            Assert.IsFalse(data.Data.IsModified);
            Assert.IsTrue(data.IsModified);
        }
    }
}