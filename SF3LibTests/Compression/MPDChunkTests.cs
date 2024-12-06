using System.Runtime.InteropServices;
using CommonLib;
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
                    using (var mpdFile = MPD_File.Create(
                        new ByteData(new ByteArray(File.ReadAllBytes(testCase.Filename))), nameGetterContext, st)) {
                        var data = mpdFile.ChunkData[5]?.Data;
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
                    using (var mpdFile = MPD_File.Create(
                        new ByteData(new ByteArray(File.ReadAllBytes(testCase.Filename))), nameGetterContext, st)) {

                        for (int i = 6; i <= 10; i++) {
                            var data = mpdFile.ChunkData[i]?.Data;
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
                    using (var mpdFile = MPD_File.Create(
                        new ByteData(new ByteArray(File.ReadAllBytes(testCase.Filename))), nameGetterContext, st)) {

                        for (int i = 6; i <= 10; i++) {
                            var data = mpdFile.ChunkData[i]?.Data;
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
    }
}
