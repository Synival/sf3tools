using static CommonLib.Utils.PixelConversion;

namespace CommonLib.Tests.Utils {
    [TestClass]
    public class PixelConversionTests {
        [TestMethod]
        public void ARGB1555toABGR1555_ConvertsCorrectly() {
            var input          = (ushort) (0x8000 | (0x1F << 10) | (0x10 << 5) | (0x05 <<  0));
            var expectedOutput = (ushort) (0x8000 | (0x1F <<  0) | (0x10 << 5) | (0x05 << 10));
            Assert.AreEqual(expectedOutput, ARGB1555toABGR1555(input));
        }

        [TestMethod]
        public void ABGR1555toARGB1555_ConvertsCorrectly() {
            var input          = (ushort) (0x8000 | (0x1F << 10) | (0x10 << 5) | (0x05 <<  0));
            var expectedOutput = (ushort) (0x8000 | (0x1F <<  0) | (0x10 << 5) | (0x05 << 10));
            Assert.AreEqual(expectedOutput, ABGR1555toARGB1555(input));
        }

        [TestMethod]
        public void ABGR1555toChannels_ReturnsExpectedChannels() {
            var input = (ushort) (0x8000 | (0x1F << 10) | (0x10 << 5) | (0x05 <<  0));
            var channels = ABGR1555toChannels(input);

            Assert.AreEqual(0xFF,      channels.a);
            Assert.AreEqual(0x1F << 3, channels.b);
            Assert.AreEqual(0x10 << 3, channels.g);
            Assert.AreEqual(0x05 << 3, channels.r);
        }

        [TestMethod]
        public void ARGB1555toChannels_ReturnsExpectedChannels() {
            var input = (ushort) (0x8000 | (0x1F << 10) | (0x10 << 5) | (0x05 <<  0));
            var channels = ARGB1555toChannels(input);

            Assert.AreEqual(0xFF,      channels.a);
            Assert.AreEqual(0x1F << 3, channels.r);
            Assert.AreEqual(0x10 << 3, channels.g);
            Assert.AreEqual(0x05 << 3, channels.b);
        }

        [TestMethod]
        public void ARGB8888toChannels_ReturnsExpectedChannels() {
            var input = 0x44332211u;
            var channels = ARGB88888oChannels(input);

            Assert.AreEqual(0x44, channels.a);
            Assert.AreEqual(0x33, channels.r);
            Assert.AreEqual(0x22, channels.g);
            Assert.AreEqual(0x11, channels.b);
        }
    }
}
