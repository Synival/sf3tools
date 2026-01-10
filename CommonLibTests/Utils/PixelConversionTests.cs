using CommonLib.Imaging;
using static CommonLib.Imaging.PixelConversion;

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

            Assert.AreEqual(0xFF,            channels.A);
            Assert.AreEqual(0x1F * 255 / 31, channels.B);
            Assert.AreEqual(0x10 * 255 / 31, channels.G);
            Assert.AreEqual(0x05 * 255 / 31, channels.R);
        }

        [TestMethod]
        public void ARGB1555toChannels_ReturnsExpectedChannels() {
            var input = (ushort) (0x8000 | (0x1F << 10) | (0x10 << 5) | (0x05 <<  0));
            var channels = ARGB1555toChannels(input);

            Assert.AreEqual(0xFF,            channels.A);
            Assert.AreEqual(0x1F * 255 / 31, channels.R);
            Assert.AreEqual(0x10 * 255 / 31, channels.G);
            Assert.AreEqual(0x05 * 255 / 31, channels.B);
        }

        [TestMethod]
        public void ARGB8888toChannels_ReturnsExpectedChannels() {
            var input = 0x44332211u;
            var channels = ARGB8888toChannels(input);

            Assert.AreEqual(0x44, channels.A);
            Assert.AreEqual(0x33, channels.R);
            Assert.AreEqual(0x22, channels.G);
            Assert.AreEqual(0x11, channels.B);
        }

        [TestMethod]
        public void PixelChannels_ToARGB1555_ConvertsCorrectly() {
            var input = new PixelChannels { A = 0x88, R = 0x77, G = 0x66, B = 0x55 };
            var output = input.ToARGB1555();

            Assert.AreEqual(0x8000, output & 0x8000);
            Assert.AreEqual(0x0E, (output >> 10) & 0x1F);
            Assert.AreEqual(0x0C, (output >> 5)  & 0x1F);
            Assert.AreEqual(0x0A, (output >> 0)  & 0x1F);
        }

        [TestMethod]
        public void PixelChannels_ToABGR1555_ConvertsCorrectly() {
            var input = new PixelChannels { A = 0x88, R = 0x77, G = 0x66, B = 0x55 };
            var output = input.ToABGR1555();

            Assert.AreEqual(0x8000, output & 0x8000);
            Assert.AreEqual(0x0A, (output >> 10) & 0x1F);
            Assert.AreEqual(0x0C, (output >> 5)  & 0x1F);
            Assert.AreEqual(0x0E, (output >> 0)  & 0x1F);
        }

        [TestMethod]
        public void PixelChannels_ToARGB8888_ConvertsCorrectly() {
            var input = new PixelChannels { A = 0x88, R = 0x77, G = 0x66, B = 0x55 };
            var output = input.ToARGB8888();

            Assert.AreEqual(0x88u, (output >> 24) & 0xFF);
            Assert.AreEqual(0x77u, (output >> 16) & 0xFF);
            Assert.AreEqual(0x66u, (output >> 8)  & 0xFF);
            Assert.AreEqual(0x55u, (output >> 0)  & 0xFF);
        }
    }
}
