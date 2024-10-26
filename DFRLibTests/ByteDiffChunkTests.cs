namespace DFRLib.Tests {
    [TestClass]
    public class ByteDiffChunkTests {
        [TestMethod]
        public void Constructor_WithAlterDFRRow_LooksLikeAlteredChunk() {
            var diff = new ByteDiffChunk("1234,abcd,dbca");
            Assert.AreEqual((ulong) 0x1234, diff.Address);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([0xAB, 0xCD], diff.BytesFrom));
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([0xDB, 0xCA], diff.BytesTo));
        }

        [TestMethod]
        public void Constructor_WithAlterDFRRowAndComment_LooksLikeAlteredChunk() {
            var diff = new ByteDiffChunk("12aF,aBcD,DbCa ; comments are fun");
            Assert.AreEqual((ulong) 0x12af, diff.Address);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([0xAB, 0xCD], diff.BytesFrom));
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([0xDB, 0xCA], diff.BytesTo));
        }

        public void Constructor_WithAppendDFRRow_LooksLikeAlteredChunk() {
            var diff = new ByteDiffChunk("12aF,,dBcA");
            Assert.AreEqual((ulong) 0x12af, diff.Address);
            Assert.AreEqual(0, diff.BytesTo.Length);
            Assert.IsTrue(Enumerable.SequenceEqual<byte>([0xDB, 0xCA], diff.BytesTo));
        }

        [TestMethod]
        public void Constructor_WithMalformedDFRRow_ThrowsException() {
            Assert.ThrowsException<ArgumentException>(() => new ByteDiffChunk("12aF,abcd,dbca,"));
            Assert.ThrowsException<ArgumentException>(() => new ByteDiffChunk("12aF,abcd"));
        }

        [TestMethod]
        public void Constructor_DFRRowWithoutRightType_ThrowsException() {
            Assert.ThrowsException<ArgumentException>(() => new ByteDiffChunk("12aF,cd,dbca"));
        }

        [TestMethod]
        public void Constructor_WithNonHexDFRRow_ThrowsException() {
            Assert.ThrowsException<ArgumentException>(() => new ByteDiffChunk("j2aF,abcd,dbca"));
            Assert.ThrowsException<ArgumentException>(() => new ByteDiffChunk("12aF,jbcd,dbca"));
            Assert.ThrowsException<ArgumentException>(() => new ByteDiffChunk("12aF,abcd,jbca"));
        }
    }
}