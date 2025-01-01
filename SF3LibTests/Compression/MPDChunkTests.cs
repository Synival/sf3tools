using System.Runtime.InteropServices;
using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.RawData;
using SF3.Types;
using static CommonLib.Utils.Compression;

namespace SF3.Tests.Compression {
    [TestClass]
    public class MPDChunkTests {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int memcmp(byte[] lhs, byte[] rhs, long count);

        private static bool ByteArraysAreEqual(byte[] lhs, byte[] rhs)
            => lhs.Length == rhs.Length && memcmp(lhs, rhs, lhs.Length) == 0;

        [Ignore]
        [TestMethod]
        public void Recompress_ConsistencyCheck_Chunk3FramesHaveValidData() {
            foreach (var st in Enum.GetValues<ScenarioType>()) {
                var mpdFiles = Directory.GetFiles(TestDataPaths.ResourcePath(st), "*.MPD");
                var testCases = mpdFiles
                    .Select(x => x.Split('/'))
                    .Select(x => x[x.Length - 1])
                    .Select(x => new TestCase(st, x))
                    .ToArray();

                var nameGetterContext = new NameGetterContext(st);
                TestCase.Run(testCases, testCase => {
                    // This particular file in Scenario 3 appears to be a leftover from Scenario 2. It's the wrong format, so skip it.
                    if (st == ScenarioType.Scenario3 && testCase.Filename.EndsWith("BTL42.MPD"))
                        return;

                    using (var mpdFile = MPD_File.Create(
                        new ByteData(new ByteArray(File.ReadAllBytes(testCase.Filename))), nameGetterContext, st)) {
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
                    }
                });
            }
        }

        [TestMethod]
        public void CompressDecompress_ConsistencyCheck_Chunk5() {
            foreach (var st in Enum.GetValues<ScenarioType>()) {
                var mpdFiles = Directory.GetFiles(TestDataPaths.ResourcePath(st), "*.MPD");
                var testCases = mpdFiles
                    .Select(x => x.Split('/'))
                    .Select(x => x[x.Length - 1])
                    .Select(x => new TestCase(st, x))
                    .ToArray();

                var nameGetterContext = new NameGetterContext(st);
                TestCase.Run(testCases, testCase => {
                    // This particular file in Scenario 3 appears to be a leftover from Scenario 2. It's the wrong format, so skip it.
                    if (st == ScenarioType.Scenario3 && testCase.Filename.EndsWith("BTL42.MPD"))
                        return;

                    using (var mpdFile = MPD_File.Create(
                        new ByteData(new ByteArray(File.ReadAllBytes(testCase.Filename))), nameGetterContext, st)) {
                        var data = mpdFile.ChunkData[5];
                        if (data == null)
                            return;

                        // Let's go!
                        var decompressed1 = Decompress(data.GetDataCopy());
                        var compressed1 = Compress(decompressed1);
                        var decompressed2 = Decompress(compressed1);

                        Assert.IsTrue(ByteArraysAreEqual(decompressed1, decompressed2));
                    }
                });
            }
        }

        [Ignore]
        [TestMethod]
        public void CompressDecompress_ConsistencyCheck_TextureChunks_WithTexturesPresent() {
            foreach (var st in Enum.GetValues<ScenarioType>()) {
                var mpdFiles = Directory.GetFiles(TestDataPaths.ResourcePath(st), "*.MPD");
                var testCases = mpdFiles
                    .Select(x => x.Split('/'))
                    .Select(x => x[x.Length - 1])
                    .Select(x => new TestCase(st, x))
                    .ToArray();

                var nameGetterContext = new NameGetterContext(st);
                TestCase.Run(testCases, testCase => {
                    // This particular file in Scenario 3 appears to be a leftover from Scenario 2. It's the wrong format, so skip it.
                    if (st == ScenarioType.Scenario3 && testCase.Filename.EndsWith("BTL42.MD"))
                        return;

                    using (var mpdFile = MPD_File.Create(
                        new ByteData(new ByteArray(File.ReadAllBytes(testCase.Filename))), nameGetterContext, st)) {

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
                    }
                });
            }
        }

        [TestMethod]
        public void CompressDecompress_ConsistencyCheck_TextureChunks_WithNoTexturesPresent() {
            foreach (var st in Enum.GetValues<ScenarioType>()) {
                var mpdFiles = Directory.GetFiles(TestDataPaths.ResourcePath(st), "*.MPD");
                var testCases = mpdFiles
                    .Select(x => x.Split('/'))
                    .Select(x => x[x.Length - 1])
                    .Select(x => new TestCase(st, x))
                    .ToArray();

                var nameGetterContext = new NameGetterContext(st);
                TestCase.Run(testCases, testCase => {
                    // This particular file in Scenario 3 appears to be a leftover from Scenario 2. It's the wrong format, so skip it.
                    if (st == ScenarioType.Scenario3 && testCase.Filename.EndsWith("BTL42.MPD"))
                        return;

                    using (var mpdFile = MPD_File.Create(
                        new ByteData(new ByteArray(File.ReadAllBytes(testCase.Filename))), nameGetterContext, st)) {

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
                    }
                });
            }
        }

        // TODO: This is failing because some files end up with extra four bytes.
        //       The file should be valid, but we should get this test passing by removing the extra four 0's.
        [TestMethod]
        public void Recompress_AfterCompressingChunk5_ChunkTableIsAccurate() {
            foreach (var st in Enum.GetValues<ScenarioType>()) {
                var mpdFiles = Directory.GetFiles(TestDataPaths.ResourcePath(st), "*.MPD");
                var testCases = mpdFiles
                    .Select(x => x.Split('/'))
                    .Select(x => x[x.Length - 1])
                    .Select(x => new TestCase(st, x))
                    .ToArray();

                var nameGetterContext = new NameGetterContext(st);
                TestCase.Run(testCases, testCase => {
                    // This particular file in Scenario 3 appears to be a leftover from Scenario 2. It's the wrong format, so skip it.
                    if (st == ScenarioType.Scenario3 && testCase.Filename.EndsWith("BTL42.MPD"))
                        return;

                    using (var mpdFile = MPD_File.Create(
                        new ByteData(new ByteArray(File.ReadAllBytes(testCase.Filename))), nameGetterContext, st)) {
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
                                Assert.AreEqual(pos, ch.ChunkAddress, "Chunk" + ch.ID + " is off");
                            pos += ch.ChunkSize;
                            if (pos % 4 != 0)
                                pos += 4 - (pos % 4);
                        }
                    }
                });
            }
        }
    }
}
