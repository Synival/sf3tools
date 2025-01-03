using CommonLib.SGL;

namespace CommonLib.Tests.SGL {
    [TestClass]
    public class FIXEDTests {
        [TestMethod]
        public void Constructor_WithRawIntInput_ProducesExpectedResults() {
            var cf1 = new FIXED(0, true);
            var cf2 = new FIXED(15554, true);
            var cf3 = new FIXED(65535, true);
            var cf4 = new FIXED(65536, true);
            var cf5 = new FIXED(-65536, true);

            Assert.AreEqual(0, cf1.RawInt);
            Assert.AreEqual(0, cf1.Int);
            Assert.AreEqual(0, cf1.Float);

            Assert.AreEqual(15554, cf2.RawInt);
            Assert.AreEqual(0, cf2.Int);
            Assert.AreEqual(0.23733520507f, cf2.Float, 0.0001f);

            Assert.AreEqual(65535, cf3.RawInt);
            Assert.AreEqual(0, cf3.Int);
            Assert.AreEqual(0.99998474121f, cf3.Float, 0.0001f);

            Assert.AreEqual(65536, cf4.RawInt);
            Assert.AreEqual(1, cf4.Int);
            Assert.AreEqual(1.00f, cf4.Float);

            Assert.AreEqual(-65536, cf5.RawInt);
            Assert.AreEqual(-1, cf5.Int);
            Assert.AreEqual(-1.00f, cf5.Float);
        }

        [TestMethod]
        public void Constructor_WithIntInput_ProducesExpectedResults() {
            var cf1 = new FIXED(-1, false);
            var cf2 = new FIXED(0, false);
            var cf3 = new FIXED(1, false);

            Assert.AreEqual(-65536, cf1.RawInt);
            Assert.AreEqual(-1, cf1.Int);
            Assert.AreEqual(-1.00f, cf1.Float);

            Assert.AreEqual(0, cf2.RawInt);
            Assert.AreEqual(0, cf2.Int);
            Assert.AreEqual(0, cf2.Float);

            Assert.AreEqual(65536, cf3.RawInt);
            Assert.AreEqual(1, cf3.Int);
            Assert.AreEqual(1.00f, cf3.Float);
        }

        [TestMethod]
        public void Constructor_WithFloatInput_ProducesExpectedResults() {
            var cf1 = new FIXED(0, 0);
            var cf2 = new FIXED(0.23733520507f, 0);
            var cf3 = new FIXED(0.99998474121f, 0);
            var cf4 = new FIXED(1.00f, 0);
            var cf5 = new FIXED(-1.00f, 0);

            Assert.AreEqual(0, cf1.RawInt);
            Assert.AreEqual(0, cf1.Int);
            Assert.AreEqual(0, cf1.Float);

            Assert.AreEqual(15554, cf2.RawInt);
            Assert.AreEqual(0, cf2.Int);
            Assert.AreEqual(0.23733520507f, cf2.Float, 0.0001f);

            Assert.AreEqual(65535, cf3.RawInt);
            Assert.AreEqual(0, cf3.Int);
            Assert.AreEqual(0.99998474121f, cf3.Float, 0.0001f);

            Assert.AreEqual(65536, cf4.RawInt);
            Assert.AreEqual(1, cf4.Int);
            Assert.AreEqual(1.00f, cf4.Float);

            Assert.AreEqual(-65536, cf5.RawInt);
            Assert.AreEqual(-1, cf5.Int);
            Assert.AreEqual(-1.00f, cf5.Float);
        }

        [TestMethod]
        public void Constructor_WithCompressedFIXEDInput_ProducesExpectedResults() {
            var cf1 = new FIXED(new CompressedFIXED(0));
            var cf2 = new FIXED(new CompressedFIXED(7777));
            var cf3 = new FIXED(new CompressedFIXED(32767));
            var cf4 = new FIXED(new CompressedFIXED(-32768));

            Assert.AreEqual(0, cf1.RawInt);
            Assert.AreEqual(0, cf1.Int);
            Assert.AreEqual(0, cf1.Float);

            Assert.AreEqual(15554, cf2.RawInt);
            Assert.AreEqual(0, cf2.Int);
            Assert.AreEqual(0.23733520507f, cf2.Float, 0.0001f);

            Assert.AreEqual(65534, cf3.RawInt);
            Assert.AreEqual(0, cf3.Int);
            Assert.AreEqual(0.99998474121f, cf3.Float, 0.0001f);

            Assert.AreEqual(-65536, cf4.RawInt);
            Assert.AreEqual(-1, cf4.Int);
            Assert.AreEqual(-1.00f, cf4.Float);
        }
    }
}
