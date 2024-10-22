using DFRLib;

namespace DFRLibTests
{
    [TestClass]
    public class ByteDiffTests
    {
        [TestMethod]
        public void Constructor_WithChangedFiles_ProducesExpectedDFR()
        {
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
        public void Constructor_WithIdenticalBytes_ProducesExpectedDFR()
        {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };
            var bytesTo   = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

            const string expectedDfr = "";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithChangedBytes_ProducesExpectedDFR()
        {
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
        public void Constructor_WithExtraBytes_ProducesExpectedDFR()
        {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };
            var bytesTo   = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80, 0x90 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

            const string expectedDfr = "6,,708090\n";
            Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithChangesAndExtraBytes_ProducesExpectedDFR()
        {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };
            var bytesTo   = new byte[] { 0x10, 0x21, 0x30, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

             const string expectedDfr =
                "1,20,22\n" +
                "3,405060,445566\n" +
                "6,,778899\n";
           Assert.AreEqual(expectedDfr, dfr);
        }

        [TestMethod]
        public void Constructor_WithMissingBytes_ProducesExpectedDFR()
        {
            var bytesFrom = new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60 };
            var bytesTo   = new byte[] { 0x10, 0x21, 0x30, 0x44, 0x55 };

            var diff = new ByteDiff(bytesFrom, bytesTo);
            var dfr = diff.ToDFR();

             const string expectedDfr =
                "1,20,22\n" +
                "3,405060,4455\n";
            Assert.AreEqual(expectedDfr, dfr);
        }
    }
}