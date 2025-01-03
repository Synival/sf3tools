using System.Runtime.InteropServices;
using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.ByteData;
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
                var mpdFiles = Directory.GetFiles(TestDataPaths.ResourcePath(st), "*.MPD");
                testCases.AddRange(mpdFiles
                    .Select(x => x.Split('/'))
                    .Select(x => x[x.Length - 1])
                    .Select(x => new TestCase(st, x))
                    // These files in Scenario 3 appear to be a leftover from Scenario 2. They're the wrong format, so skip them.
                    .Where(x => {
                        return (x.Scenario != ScenarioType.Scenario3)
                            ? true
                            : !x.Filename.EndsWith("BTL42.MPD") &&
                              !x.Filename.EndsWith("MUHASI.MPD") &&
                              !x.Filename.EndsWith("SNIOKI.MPD");
                    })
                    .ToArray()
                );
            }

            return testCases.ToArray();
        }

        private static void RunOnAllTestCases(Action<TestCase, MPD_File> func) {
            var testCases = CreateAllTestCases();
            TestCase.Run(testCases, testCase => {
                var scn = testCase.Scenario;
                using (var mpdFile = MPD_File.Create(
                    new SF3.ByteData.ByteData(new ByteArray(File.ReadAllBytes(testCase.Filename))), new NameGetterContext(scn), scn)
                ) {
                    func(testCase, mpdFile);
                }
            });
        }

        [Ignore]
        [TestMethod]
        public void Recompress_ConsistencyCheck_Chunk3FramesHaveValidData() {
            RunOnAllTestCases((testCase, mpdFile) => {
                if (mpdFile.Chunk3Frames == null || mpdFile.Chunk3Frames.Count == 0)
                    return;

                _ = mpdFile.Recompress(onlyModified: false);
                var allFrames = mpdFile.TextureAnimations.Rows
                    .SelectMany(x => x.Frames)
                    .GroupBy(x => x.CompressedTextureOffset)
                    .ToDictionary(x => x.Key, x => x.First());

                foreach (var c3fKv in mpdFile.Chunk3Frames) {
                    var frame = allFrames[(uint) c3fKv.Offset];
                    var compressedByteArray = c3fKv.Data.GetDataCopy();
                    var bytesPerPixel = (frame.AssumedPixelFormat == TexturePixelFormat.ABGR1555) ? 2 : 1;
                    var expectedUncompressedDataSize = frame.Width * frame.Height * bytesPerPixel;

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
                if (mpdFile.TileSurfaceHeightmapRows == null)
                    return;

                var rng = new Random();
                var rngBytes = new byte[4];
                foreach (var row in mpdFile.TileSurfaceHeightmapRows.Rows) {
                    for (var x = 0; x < 64; x++) {
                        rng.NextBytes(rngBytes);
                        row.SetHeights(x, rngBytes.Select(x => x / 16f).ToArray());
                    }
                }

                _ = mpdFile.Recompress(onlyModified: true);

                var pos = 0x292100;
                foreach (var ch in mpdFile.ChunkHeader.Rows) {
                    if (ch.ChunkAddress != 0)
                        Assert.AreEqual(pos, ch.ChunkAddress, "Chunk[" + ch.ID + "] is off");
                    pos += ch.ChunkSize;
                    if (pos % 4 != 0)
                        pos += 4 - (pos % 4);
                }

                // Attempt to create the file to make sure it's not corrupted.
                var recreatedMpdFile = MPD_File.Create(new SF3.ByteData.ByteData(new ByteArray(mpdFile.Data.GetDataCopy())), mpdFile.NameGetterContext, mpdFile.Scenario);
            });
        }
    }
}
