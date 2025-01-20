using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.Types;
using static SF3.Tests.TestDataPaths;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class MPD_FileTests {
        private static MPD_File MakeFile() {
            var nameGetters = Enum
                .GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            var data = new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(ResourcePath(ScenarioType.Scenario1, "BTL02.MPD"))));
            return MPD_File.Create(data, nameGetters);
        }

        [TestMethod]
        public void Create_NothingIsModified() {
            // Arrange + Act
            var mpdFile = MakeFile();

            // Assert
            Assert.IsFalse(mpdFile.IsModified);
            foreach (var cd in mpdFile.ChunkData) {
                if (cd != null) {
                    Assert.IsFalse(cd.IsModified);
                    Assert.IsFalse(cd.DecompressedData.IsModified);
                }
            }
            foreach (var c3fKv in mpdFile.Chunk3Frames) {
                Assert.IsFalse(c3fKv.Data.IsModified);
                Assert.IsFalse(c3fKv.Data.DecompressedData.IsModified);
            }
        }

        [TestMethod]
        public void ChunkData_WithDifferentOriginalCompressionAlgorithmForData_Recompress_UpdatesCompressedData() {
            // Arrange
            var mpdFile = MakeFile();
            var data = new CompressedData(new ByteArray(mpdFile.ChunkData[5].GetDataCopy()));
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsFalse(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);

            // Act
            var recompressResult = data.Recompress();

            // Assert
            Assert.IsTrue(recompressResult);
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsTrue(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);
        }

        [TestMethod]
        public void ChunkData_WithDifferentOriginalCompressionAlgorithmForData_RecompressAgain_IsModifiedIsFalse() {
            // Arrange
            var mpdFile = MakeFile();
            var data = new CompressedData(new ByteArray(mpdFile.ChunkData[5].GetDataCopy()));
            data.Recompress();
            data.IsModified = false;
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsFalse(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);

            // Act
            var recompressResult = data.Recompress();

            // Assert
            Assert.IsTrue(recompressResult);
            Assert.IsFalse(data.NeedsRecompression);
            Assert.IsFalse(data.IsModified);
            Assert.IsFalse(data.DecompressedData.IsModified);
        }

        [TestMethod]
        public void Create_WithoutChanges_NothingHasIsModifiedFlag() {
            // Arrange + Act
            var data = MakeFile();

            // Assert
            foreach (var ce in data.ChunkData.Where(x => x != null))
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

            for (var i = 0; i < data.ChunkData.Length; i++) {
                var chunkData = data.ChunkData[i];
                if (chunkData == null)
                    continue;

                if (!chunkData.IsCompressed)
                    Assert.AreEqual(false, chunkData.IsModified);
                else {
                    // NOTE: Recompressing an actual .MPD file uses a different algorithm than originally used
                    //       so the content of the chunks will have been modified from the original test data.
                    //       The MPD_File should be an a modified state.
                    //       Chunks 9 and 10 are actually unmodified, how about that!
                    var expectedResult = i == 9 || i == 10 ? false : true;
                    Assert.IsFalse(chunkData.NeedsRecompression);
                    Assert.IsFalse(chunkData.DecompressedData.IsModified);
                    Assert.AreEqual(expectedResult, chunkData.IsModified);
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
            foreach (var ce in data.ChunkData.Where(x => x != null)) {
                Assert.IsFalse(ce.IsModified);
                Assert.IsFalse(ce.DecompressedData.IsModified);
            }
            Assert.IsFalse(data.Data.IsModified);
        }

        [TestMethod]
        public void TextureChunks_DataIsModified_SetsIfModifiedFlags() {
            // Arrange
            var data = MakeFile();
            Assert.IsFalse(data.ChunkData[6].IsModified);
            Assert.IsFalse(data.ChunkData[6].DecompressedData.IsModified);
            Assert.IsFalse(data.Data.IsModified);
            Assert.IsFalse(data.IsModified);

            // Act
            data.TextureCollections[0].TextureTable[0].Width *= 2;

            // Assert
            Assert.IsTrue(data.ChunkData[6].IsModified);
            Assert.IsTrue(data.ChunkData[6].DecompressedData.IsModified);
            Assert.IsTrue(data.Data.IsModified);
            Assert.IsTrue(data.IsModified);
        }

        [TestMethod]
        public void Finish_WithoutChanges_NothingHasIsModifiedFlag() {
            // Arrange
            var data = MakeFile();

            // Act
            data.Finish();

            // Assert
            foreach (var ce in data.ChunkData.Where(x => x != null)) {
                Assert.IsFalse(ce.IsModified);
                Assert.IsFalse(ce.DecompressedData.IsModified);
            }
            Assert.IsFalse(data.Data.IsModified);
        }

        [TestMethod]
        public void Finish_WithChanges_ExpectedDataHasIsModifiedFlag() {
            // Arrange
            var data = MakeFile();
            data.TextureCollections[0].TextureTable[0].Width *= 2;

            // Act
            data.Finish();

            // Assert
            for (var i = 0; i < data.ChunkData.Length; i++) {
                var chunkData = data.ChunkData[i];
                if (chunkData == null)
                    continue;

                if (chunkData.IsCompressed)
                    Assert.IsFalse(chunkData.DecompressedData.IsModified);

                var expectedModifiedFlag = i == 6;
                Assert.AreEqual(expectedModifiedFlag, chunkData.IsModified);
            }

            Assert.IsTrue(data.Data.IsModified);
            Assert.IsTrue(data.IsModified);
        }

        [TestMethod]
        public void ChunkData_SettingIsModifiedToTrue_SetsDataIsModifiedToTrue() {
            for (var chunkToTest = 0; chunkToTest < 32; chunkToTest++) {
                // Arrange
                var data = MakeFile();
                var chunkData = data.ChunkData[chunkToTest];
                if (chunkData == null)
                    continue;
                Assert.IsFalse(data.Data.IsModified);

                // Act
                chunkData.IsModified = true;

                // Assert
                Assert.IsTrue(data.Data.IsModified);
                Assert.IsTrue(data.IsModified);
            }
        }

        [TestMethod]
        public void Data_IsModified_SettingToFalseWhenNoRecompressionIsRequired_IsSetToFalse() {
            // Arrange
            var data = MakeFile();
            data.Data.IsModified = true;
            Assert.IsFalse(data.ChunkData.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkData.Where(x => x != null).Any(x => x.IsModified));

            // Act
            data.Data.IsModified = false;

            // Assert
            Assert.IsFalse(data.ChunkData.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkData.Where(x => x != null).Any(x => x.IsModified));
            Assert.IsFalse(data.Data.IsModified);
            Assert.IsFalse(data.IsModified);
        }

        [TestMethod]
        public void CompressedData_NeedsRecompressionSet_ResultsInExpectedDataState() {
            // Arrange
            var data = MakeFile();
            data.Data.IsModified = true;
            Assert.IsFalse(data.ChunkData.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkData.Where(x => x != null).Any(x => x.IsModified));
            var chunkData = data.ChunkData[5];

            // Act
            chunkData.NeedsRecompression = true;

            // Assert
            Assert.IsFalse(data.ChunkData.Where(x => x != null && x != chunkData).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkData.Where(x => x != null && x != chunkData).Any(x => x.IsModified));

            Assert.IsTrue(chunkData.NeedsRecompression);
            Assert.IsTrue(chunkData.IsModified);

            Assert.IsTrue(data.Data.IsModified);
            Assert.IsTrue(data.IsModified);
        }

        [TestMethod]
        public void Data_IsModifiedSetToFalseWhenRecompressionIsRequired_IsStillTrue() {
            // Arrange
            var data = MakeFile();
            data.Data.IsModified = true;
            Assert.IsFalse(data.ChunkData.Where(x => x != null).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkData.Where(x => x != null).Any(x => x.IsModified));
            var chunkData = data.ChunkData[5];
            chunkData.NeedsRecompression = true;

            // Act
            data.IsModified = false;

            // Assert
            Assert.IsTrue(chunkData.NeedsRecompression);
            Assert.IsTrue(chunkData.IsModified);
            Assert.IsFalse(data.ChunkData.Where(x => x != null && x != chunkData).Any(x => x.NeedsRecompression));
            Assert.IsFalse(data.ChunkData.Where(x => x != null && x != chunkData).Any(x => x.IsModified));
            Assert.IsFalse(data.Data.IsModified);
            Assert.IsTrue(data.IsModified);
        }
    }
}