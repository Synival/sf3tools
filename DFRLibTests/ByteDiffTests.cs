using System.Text;

namespace DFRLib.Tests {
    [TestClass]
    public class ByteDiffTests {
        [TestMethod]
        public void Constructor_WithChangedFiles_ProducesSpecificDFR() {
            const string fileFromPath = "../../../TestData/X033_Orig.BIN"; // (output file goes here!)
            const string fileToPath = "../../../TestData/X033_Updated.BIN"; // (input file goes here!)

            var diff = new ByteDiff(fileFromPath, fileToPath);
            var dfr = diff.ToDFR();

            const string expectedDfr =
                "1315,0b0f141617191a,1b1f242627292a\n" +
                "5502,0a,47\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithIdenticalBytes_ProducesBlankDFR() {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };
            var bytesTo   = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

            const string expectedDfr = "";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithChangedBytes_ProducesDFRWithExpectedChanges() {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };
            var bytesTo   = new byte[] { 0x10, 0x22, 0x30, 0x44, 0x55, 0x66 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

            const string expectedDfr =
                "1,20,22\n" +
                "3,405060,445566\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithAppendedBytes_ProducesDFRWithAppend() {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };
            var bytesTo   = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80, 0x90 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

            const string expectedDfr = "6,,708090\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithChangedAndAppenedBytes_ProducesDFRWithChangedAndAppendedBytes() {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };
            var bytesTo   = new byte[] { 0x10, 0x22, 0x30, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

            const string expectedDfr =
                "1,20,22\n" +
                "3,405060,445566\n" +
                "6,,778899\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithMissingBytes_ProducesDFRForOnlyBytesInToData() {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };
            var bytesTo   = new byte[] { 0x10, 0x22, 0x30, 0x44, 0x55 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

            const string expectedDfr =
                "1,20,22\n" +
                "3,4050,4455\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithZeroesBeforeAppend_ProducesDFRWithZeroesIgnored() {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40 };
            var bytesTo   = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x00, 0x00, 0x50, 0x60 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

            const string expectedDfr =
                "6,,5060\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithAppendWithTrailingZeroes_ProducesDFRWithoutZeroesIgnored() {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40 };
            var bytesTo   = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x00, 0x00 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

            const string expectedDfr =
                "4,,50600000\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithMultipleAppendChunksWithZeroes_ProducesExpectedDFR() {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40 };
            var bytesTo   = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x00, 0x00, 0x70, 0x80, 0x00, 0x00 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

            const string expectedDfr =
                "4,,5060\n" +
                "8,,70800000\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithMultipleAppendChunksWithZeroes_AndCombineOption_ProducesExpectedDFR() {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40 };
            var bytesTo   = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x00, 0x00, 0x70, 0x80, 0x00, 0x00 };

            var diff = new ByteDiff(bytesFrom, bytesTo, new ByteDiffChunkBuilderOptions { CombineAppendedChunks = true });
            var dfr = diff.ToDFR();

            const string expectedDfr =
                "4,,5060000070800000\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_FromByteStream_ProducesExpectedDFR() {
            var data =
                "; do something\n" +
                "12FF,0081,0182\n" +
                "\r\n" +
                "4321,,DEADBEEF\r\n" +
                "    " +
                "5000,,01";

            var diff = new ByteDiff(new MemoryStream(Encoding.UTF8.GetBytes(data)));
            var dfr = diff.ToDFR();

            const string expectedDfr =
                "12ff,0081,0182\n" +
                "4321,,deadbeef\n" +
                "5000,,01\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void ApplyTo_WithChunksFromDFR_SetsExpectedBytes() {
            var bytes = new byte[] {
                0x00, 0x00, 0x01, 0x81,
                0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF
            };
            var expectedBytes = new byte[] {
                0x00, 0x00, 0x02, 0x82,
                0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0xFF, 0xFF, 0xFF,
                0x00, 0x00, 0x00, 0x00,
                0xDE, 0xAD, 0xBE, 0xEF
            };

            var dfrData =
                "02,0181,0282\n" +
                "10,,DEADBEEF";
            var diff = new ByteDiff(new MemoryStream(Encoding.UTF8.GetBytes(dfrData)));

            var resultBytes = diff.ApplyTo(bytes);
            Assert.IsTrue(Enumerable.SequenceEqual(expectedBytes, resultBytes));
        }

        [TestMethod]
        public void ApplyTo_WithUnexpectedData_Throws() {
            var bytes = new byte[] { 0x00, 0x01, 0x02, 0x03 };
            var dfrData = "02,0205,0203";
            var diff = new ByteDiff(new MemoryStream(Encoding.UTF8.GetBytes(dfrData)));

            _ = Assert.ThrowsException<ArgumentException>(() => diff.ApplyTo(bytes));
        }
    }
}