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
            var channels = ARGB8888ToChannels(input);

            Assert.AreEqual(0x44, channels.a);
            Assert.AreEqual(0x33, channels.r);
            Assert.AreEqual(0x22, channels.g);
            Assert.AreEqual(0x11, channels.b);
        }

        [TestMethod]
        public void PixelChannels_ToARGB1555_ConvertsCorrectly() {
            var input = new PixelChannels { a = 0x88, r = 0x77, g = 0x66, b = 0x55 };
            var output = input.ToARGB1555();

            Assert.AreEqual(0x8000, output & 0x8000);
            Assert.AreEqual(0x0E, (output >> 10) & 0x1F);
            Assert.AreEqual(0x0C, (output >> 5)  & 0x1F);
            Assert.AreEqual(0x0A, (output >> 0)  & 0x1F);
        }

        [TestMethod]
        public void PixelChannels_ToABGR1555_ConvertsCorrectly() {
            var input = new PixelChannels { a = 0x88, r = 0x77, g = 0x66, b = 0x55 };
            var output = input.ToABGR1555();

            Assert.AreEqual(0x8000, output & 0x8000);
            Assert.AreEqual(0x0A, (output >> 10) & 0x1F);
            Assert.AreEqual(0x0C, (output >> 5)  & 0x1F);
            Assert.AreEqual(0x0E, (output >> 0)  & 0x1F);
        }

        [TestMethod]
        public void PixelChannels_ToARGB8888_ConvertsCorrectly() {
            var input = new PixelChannels { a = 0x88, r = 0x77, g = 0x66, b = 0x55 };
            var output = input.ToARGB8888();

            Assert.AreEqual(0x88u, (output >> 24) & 0xFF);
            Assert.AreEqual(0x77u, (output >> 16) & 0xFF);
            Assert.AreEqual(0x66u, (output >> 8)  & 0xFF);
            Assert.AreEqual(0x55u, (output >> 0)  & 0xFF);
        }
    }
}
