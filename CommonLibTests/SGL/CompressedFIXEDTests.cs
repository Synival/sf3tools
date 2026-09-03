using CommonLib.SGL;

namespace CommonLib.Tests.SGL {
    [TestClass]
    public class CompressedFIXEDTests {
        [TestMethod]
        public void Constructor_WithRawShortInput_12_ProducesExpectedResults() {
            var cf1 = new CompressedFIXED(0, 12);
            var cf2 = new CompressedFIXED(972, 12);
            var cf3 = new CompressedFIXED(4096, 12);
            var cf4 = new CompressedFIXED(-4096, 12);

            Assert.AreEqual(0, cf1.RawShort);
            Assert.AreEqual(0, cf1.Float);

            Assert.AreEqual(972, cf2.RawShort);
            Assert.AreEqual(0.237304688f, cf2.Float, 0.0001f);

            Assert.AreEqual(4096, cf3.RawShort);
            Assert.AreEqual(1.00f, cf3.Float, 0.0000f);

            Assert.AreEqual(-4096, cf4.RawShort);
            Assert.AreEqual(-1.00f, cf4.Float, 0.0000f);
        }

        [TestMethod]
        public void Constructor_WithFloatInput_12_ProducesExpectedResults() {
            var cf1 = new CompressedFIXED(0.0f, 12, 0);
            var cf2 = new CompressedFIXED(0.23733520507f, 12, 0);
            var cf3 = new CompressedFIXED(1.00f, 12, 0);
            var cf4 = new CompressedFIXED(-1.00f, 12, 0);

            Assert.AreEqual(0, cf1.RawShort);
            Assert.AreEqual(0, cf1.Float);

            Assert.AreEqual(972, cf2.RawShort);
            Assert.AreEqual(0.237304688f, cf2.Float, 0.0001f);

            Assert.AreEqual(4096, cf3.RawShort);
            Assert.AreEqual(1.00f, cf3.Float, 0.0000f);

            Assert.AreEqual(-4096, cf4.RawShort);
            Assert.AreEqual(-1.00f, cf4.Float, 0.0000f);
        }

        [TestMethod]
        public void Constructor_WithFIXEDInput_12_ProducesExpectedResults() {
            var cf1 = new CompressedFIXED(new FIXED(0, true), 12);
            var cf2 = new CompressedFIXED(new FIXED(15555, true), 12);
            var cf3 = new CompressedFIXED(new FIXED(65536, true), 12);
            var cf4 = new CompressedFIXED(new FIXED(-65536, true), 12);

            Assert.AreEqual(0, cf1.RawShort);
            Assert.AreEqual(0, cf1.Float);

            Assert.AreEqual(972, cf2.RawShort);
            Assert.AreEqual(0.237304688f, cf2.Float, 0.0001f);

            Assert.AreEqual(4096, cf3.RawShort);
            Assert.AreEqual(1.00f, cf3.Float, 0.0000f);

            Assert.AreEqual(-4096, cf4.RawShort);
            Assert.AreEqual(-1.00f, cf4.Float, 0.0000f);
        }

        [TestMethod]
        public void Constructor_WithRawShortInput_13_ProducesExpectedResults() {
            var cf1 = new CompressedFIXED(0, 13);
            var cf2 = new CompressedFIXED(972, 13);
            var cf3 = new CompressedFIXED(4096, 13);
            var cf4 = new CompressedFIXED(-4096, 13);

            Assert.AreEqual(0, cf1.RawShort);
            Assert.AreEqual(0, cf1.Float);

            Assert.AreEqual(972, cf2.RawShort);
            Assert.AreEqual(0.118652344f, cf2.Float, 0.0001f);

            Assert.AreEqual(4096, cf3.RawShort);
            Assert.AreEqual(0.50f, cf3.Float, 0.0000f);

            Assert.AreEqual(-4096, cf4.RawShort);
            Assert.AreEqual(-0.50f, cf4.Float, 0.0000f);
        }

        [TestMethod]
        public void Constructor_WithFloatInput_13_ProducesExpectedResults() {
            var cf1 = new CompressedFIXED(0.0f, 13, 0);
            var cf2 = new CompressedFIXED(0.23733520507f, 13, 0);
            var cf3 = new CompressedFIXED(1.00f, 13, 0);
            var cf4 = new CompressedFIXED(-1.00f, 13, 0);

            Assert.AreEqual(0, cf1.RawShort);
            Assert.AreEqual(0, cf1.Float);

            Assert.AreEqual(1944, cf2.RawShort);
            Assert.AreEqual(0.237304688f, cf2.Float, 0.0001f);

            Assert.AreEqual(8192, cf3.RawShort);
            Assert.AreEqual(1.00f, cf3.Float, 0.0000f);

            Assert.AreEqual(-8192, cf4.RawShort);
            Assert.AreEqual(-1.00f, cf4.Float, 0.0000f);
        }

        [TestMethod]
        public void Constructor_WithFIXEDInput_13_ProducesExpectedResults() {
            var cf1 = new CompressedFIXED(new FIXED(0, true), 13);
            var cf2 = new CompressedFIXED(new FIXED(15555, true), 13);
            var cf3 = new CompressedFIXED(new FIXED(65536, true), 13);
            var cf4 = new CompressedFIXED(new FIXED(-65536, true), 13);

            Assert.AreEqual(0, cf1.RawShort);
            Assert.AreEqual(0, cf1.Float);

            Assert.AreEqual(1944, cf2.RawShort);
            Assert.AreEqual(0.237304688f, cf2.Float, 0.0001f);

            Assert.AreEqual(8192, cf3.RawShort);
            Assert.AreEqual(1.00f, cf3.Float, 0.0000f);

            Assert.AreEqual(-8192, cf4.RawShort);
            Assert.AreEqual(-1.00f, cf4.Float, 0.0000f);
        }
    }
}
