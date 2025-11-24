using CommonLib.Arrays;
using CommonLib.NamedValues;
using CommonLib.Tests;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.Types;
using static SF3.Tests.TestDataPaths;

namespace SF3.Tests.Models.Files {
    [TestClass]
    public class MPD_FileTests {
        private static MPD_File MakeFile(ScenarioType scenario = ScenarioType.Scenario1) {
            var nameGetters = Enum
                .GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            var getResourcePath = () => {
                switch (scenario) {
                    case ScenarioType.Scenario1:
                        return ResourcePath(scenario, "BTL02.MPD") ?? "";
                    case ScenarioType.Scenario2:
                        return ResourcePath(scenario, "BTL42.MPD") ?? "";
                    case ScenarioType.Scenario3:
                        return ResourcePath(scenario, "BTL87.MPD") ?? "";
                    case ScenarioType.PremiumDisk:
                        return ResourcePath(scenario, "PD_MAP.MPD") ?? "";
                    default:
                        throw new ArgumentException($"Unhandled scenario '{scenario}'");
                }
            };

            var data = new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(getResourcePath())));
            return MPD_File.Create(data, nameGetters, scenario);
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
        public void ChunkData_WithDifferentOriginalCompressionAlgorithmForData_Recompress_PerformsNoUpdates() {
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
            Assert.IsFalse(data.IsModified);
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

        public class ValueTestCase : TestCase {
            public ValueTestCase(int value) : base(value.ToString()) {
                Value = value;
            }

            public readonly int Value;
        }

        [TestMethod]
        public void Recompress_WithoutChanges_HasExpectedIsModifiedFlags() {
            // Arrange
            var data = MakeFile();

            // Act
            data.RecompressChunks(onlyModified: false);

            var range = Enumerable.Range(0, data.ChunkData.Length).Select(x => new ValueTestCase(x)).ToArray();
            TestCase.Run(range, (testCase) => {
                var chunkData = data.ChunkData[testCase.Value];
                if (chunkData == null)
                    return;

                if (!chunkData.IsCompressed)
                    Assert.AreEqual(false, chunkData.IsModified);
                else {
                    // Some chunks are always modified due to differences in the compression algorithm that can't be reproduce.
                    var shouldBeModified = testCase.Value == 13 || testCase.Value == 17;
                    Assert.IsFalse(chunkData.NeedsRecompression);
                    Assert.IsFalse(chunkData.DecompressedData.IsModified);
                    Assert.AreEqual(shouldBeModified, chunkData.IsModified);
                }
            });

            Assert.IsTrue(data.Data.IsModified);
            Assert.IsTrue(data.IsModified);
        }

        [TestMethod]
        public void Recompress_WithoutChanges_ASecondTime_ResultsInExpectedState() {
            // Arrange
            var data = MakeFile();
            data.RecompressChunks(onlyModified: false);
            data.IsModified = false;
            Assert.IsFalse(data.Data.IsModified);

            // Act
            data.RecompressChunks(onlyModified: false);

            // Assert
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
            data.TextureChunks[0].TextureTable[0].Width *= 2;

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
            data.TextureChunks[0].TextureTable[0].Width *= 2;

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

        [TestMethod]
        public void Models_HasExpectedCounts() {
            var data = MakeFile();
            var models = data.ModelCollections[CollectionType.Primary].Models.ToArray();

            Assert.AreEqual(14, models.Length);

            var sarabandModel = models[7];
            Assert.AreEqual(289, sarabandModel.Vertices.Count);
            Assert.AreEqual(193, sarabandModel.Faces.Count);
        }
    }
}