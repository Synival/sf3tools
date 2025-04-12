using System.Runtime.InteropServices;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.Types;
using static CommonLib.Utils.Compression;

namespace SF3.Tests.Compression {
    [TestClass]
    public class MPDChunkTests {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int memcmp(byte[] lhs, byte[] rhs, long count);

        private static bool ByteArraysAreEqual(byte[] lhs, byte[] rhs)
            => lhs.Length == rhs.Length && memcmp(lhs, rhs, lhs.Length) == 0;

        private static TestCase[] CreateAllTestCases() {
            var testCases = new List<TestCase>();

            foreach (var st in Enum.GetValues<ScenarioType>()) {
                var resourcePath = TestDataPaths.ResourcePath(st);
                if (resourcePath == null)
                    continue;

                var mpdFiles = Directory.GetFiles(resourcePath, "*.MPD");
                testCases.AddRange(mpdFiles
                    .Select(x => x.Split('/'))
                    .Select(x => x[x.Length - 1])
                    .Select(x => new TestCase(st, x))
                    .ToArray()
                );
            }

            return testCases.ToArray();
        }

        private static void RunOnAllTestCases(Action<TestCase, MPD_File> func) {
            var nameGetters = Enum
                .GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));
            var testCases = CreateAllTestCases();
            TestCase.Run(testCases, testCase => {
                var byteData = new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(testCase.Filename)));
                using (var mpdFile = MPD_File.Create(byteData, nameGetters))
                    func(testCase, mpdFile);
            });
        }

        [Ignore]
        [TestMethod]
        public void Recompress_ConsistencyCheck_Chunk3FramesHaveValidData() {
            RunOnAllTestCases((testCase, mpdFile) => {
                if (mpdFile.Chunk3Frames == null || mpdFile.Chunk3Frames.Count == 0)
                    return;

                _ = mpdFile.Recompress(onlyModified: false);
                var allFrames = mpdFile.TextureAnimations
                    .SelectMany(x => x.FrameTable)
                    .GroupBy(x => x.CompressedImageDataOffset)
                    .ToDictionary(x => x.Key, x => x.First());

                foreach (var c3fKv in mpdFile.Chunk3Frames) {
                    var frame = allFrames[(uint) c3fKv.Offset];
                    var compressedByteArray = c3fKv.Data.GetDataCopy();
                    var bytesPerPixel = frame.PixelFormat.BytesPerPixel();
                    var expectedUncompressedDataSize = frame.UncompressedImageDataSize;

                    var compressedData = new CompressedData(new ByteArray(compressedByteArray), expectedUncompressedDataSize);
                    Assert.AreEqual(expectedUncompressedDataSize, compressedData.DecompressedData.Length);
                }
            });
        }

        [TestMethod]
        public void CompressDecompress_ConsistencyCheck_Chunk5() {
            RunOnAllTestCases((testCase, mpdFile) => {
                var data = mpdFile.ChunkData[5];
                if (data == null)
                    return;

                // Let's go!
                var decompressed1 = Decompress(data.GetDataCopy());
                var compressed1 = Compress(decompressed1);
                var decompressed2 = Decompress(compressed1);

                Assert.IsTrue(ByteArraysAreEqual(decompressed1, decompressed2));
            });
        }

        [Ignore]
        [TestMethod]
        public void CompressDecompress_ConsistencyCheck_TextureChunks_WithTexturesPresent() {
            RunOnAllTestCases((testCase, mpdFile) => {
                for (int i = 6; i <= 10; i++) {
                    var data = mpdFile.ChunkData[i];
                    if (data == null || data.Length == 8)
                        continue;

                    // Let's go!
                    var decompressed1 = Decompress(data.GetDataCopy());
                    var compressed1 = Compress(decompressed1);
                    var decompressed2 = Decompress(compressed1);

                    Assert.IsTrue(ByteArraysAreEqual(decompressed1, decompressed2), "Chunk" + i.ToString("D2") + " failed (" + data.Length + " bytes)");
                }
            });
        }

        [TestMethod]
        public void CompressDecompress_ConsistencyCheck_TextureChunks_WithNoTexturesPresent() {
            RunOnAllTestCases((testCase, mpdFile) => {
                for (int i = 6; i <= 10; i++) {
                    var data = mpdFile.ChunkData[i];
                    if (data == null || data.Length != 8)
                        continue;

                    // Let's go!
                    var decompressed1 = Decompress(data.GetDataCopy());
                    var compressed1 = Compress(decompressed1);
                    var decompressed2 = Decompress(compressed1);

                    Assert.IsTrue(ByteArraysAreEqual(decompressed1, decompressed2), "Chunk" + i.ToString("D2") + " failed (" + data.Length + " bytes)");
                }
            });
        }

        [TestMethod]
        public void Recompress_AfterCompressingChunk5_ChunkTableIsAccurate() {
            RunOnAllTestCases((testCase, mpdFile) => {
                if (mpdFile.Chunk3Frames == null || mpdFile.Chunk3Frames.Count == 0)
                    return;
                if (mpdFile.Surface == null)
                    return;

                var rng = new Random();
                var rngBytes = new byte[4];
                foreach (var row in mpdFile.Surface.HeightmapRowTable) {
                    for (var x = 0; x < 64; x++) {
                        rng.NextBytes(rngBytes);
                        row.SetQuadHeights(x, rngBytes.Select(x => x / 16f).ToArray());
                    }
                }

                _ = mpdFile.Recompress(onlyModified: true);

                var pos = 0x292100;
                foreach (var ch in mpdFile.ChunkHeader) {
                    if (ch.ChunkRAMAddress != 0)
                        Assert.AreEqual(pos, ch.ChunkRAMAddress, "Chunk[" + ch.ID + "] is off");
                    pos += ch.ChunkSize;
                    if (pos % 4 != 0)
                        pos += 4 - (pos % 4);
                }

                // Attempt to create the file to make sure it's not corrupted.
                var nameGetters = new Dictionary<ScenarioType, INameGetterContext>() {
                    { mpdFile.Scenario, mpdFile.NameGetterContext }
                };
                var recreatedMpdFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(mpdFile.Data.GetDataCopy())), nameGetters);
            });
        }

        [TestMethod]
        public void RebuildChunkTable_WithUncompressedChunkMovedToEnd_ChunkTableIsAccurate() {
            RunOnAllTestCases((testCase, mpdFile) => {
                var firstUncompressedChunk = mpdFile.ChunkHeader.FirstOrDefault(x => x.Exists && x.CompressionType != CompressionType.Compressed);
                if (firstUncompressedChunk == null)
                    return;

                // Build a new MPD file with the first compressed chunk moved to the end of the file.
                var oldChunkPos = firstUncompressedChunk.ChunkRAMAddress - 0x290000;
                var newChunkPos = mpdFile.Data.Length;
                mpdFile.Data.Data.SetDataAtTo(newChunkPos, 0, mpdFile.Data.GetDataCopyAt(oldChunkPos, firstUncompressedChunk.ChunkSize));

                var nameGetters = new Dictionary<ScenarioType, INameGetterContext>() {
                    { mpdFile.Scenario, mpdFile.NameGetterContext }
                };
                firstUncompressedChunk.ChunkRAMAddress = newChunkPos + 0x290000;
                mpdFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(mpdFile.Data.GetDataCopy())), nameGetters);

                // Act
                var orderedChunkHeadersOrig = mpdFile.ChunkHeader
                    .Where(x => x.ChunkRAMAddress != 0)
                    .OrderBy(x => x.ChunkRAMAddress)
                    .ToArray();

                mpdFile.RebuildChunkTable(onlyModified: true);

                var pos = 0x2100;
                var orderedChunkHeaders = mpdFile.ChunkHeader
                    .Where(x => x.ChunkRAMAddress != 0)
                    .OrderBy(x => x.ChunkRAMAddress)
                    .ThenBy(x => x.ChunkSize)
                    .ThenBy(x => x.ID)
                    .ToArray();

                foreach (var ch in orderedChunkHeaders) {
                    if (ch.ChunkRAMAddress != 0)
                        Assert.AreEqual(pos, ch.ChunkFileAddress, "Chunk[" + ch.ID + "] is off");
                    pos += ch.ChunkSize;
                    if (pos % 4 != 0)
                        pos += 4 - (pos % 4);
                }

                // Attempt to create the file to make sure it's not corrupted.
                var recreatedMpdFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(mpdFile.Data.GetDataCopy())), nameGetters);
            });
        }
    }
}
