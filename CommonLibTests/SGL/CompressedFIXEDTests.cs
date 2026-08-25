using CommonLib.SGL;

namespace CommonLib.Tests.SGL {
    [TestClass]
    public class CompressedFIXEDTests {
        [TestMethod]
        public void Constructor_WithRawShortInput_ProducesExpectedResults() {
            var cf1 = new CompressedFIXED(0);
            var cf2 = new CompressedFIXED(972);
            var cf3 = new CompressedFIXED(4096);
            var cf4 = new CompressedFIXED(-4096);

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
        public void Constructor_WithFloatInput_ProducesExpectedResults() {
            var cf1 = new CompressedFIXED(0.0f, 0);
            var cf2 = new CompressedFIXED(0.23733520507f, 0);
            var cf3 = new CompressedFIXED(1.00f, 0);
            var cf4 = new CompressedFIXED(-1.00f, 0);

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
        public void Constructor_WithFIXEDInput_ProducesExpectedResults() {
            var cf1 = new CompressedFIXED(new FIXED(0, true));
            var cf2 = new CompressedFIXED(new FIXED(15555, true));
            var cf3 = new CompressedFIXED(new FIXED(65536, true));
            var cf4 = new CompressedFIXED(new FIXED(-65536, true));

            Assert.AreEqual(0, cf1.RawShort);
            Assert.AreEqual(0, cf1.Float);

            Assert.AreEqual(972, cf2.RawShort);
            Assert.AreEqual(0.237304688f, cf2.Float, 0.0001f);

            Assert.AreEqual(4096, cf3.RawShort);
            Assert.AreEqual(1.00f, cf3.Float, 0.0000f);

            Assert.AreEqual(-4096, cf4.RawShort);
            Assert.AreEqual(-1.00f, cf4.Float, 0.0000f);
        }
    }
}
