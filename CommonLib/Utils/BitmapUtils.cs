using CommonLib.Imaging;
using static CommonLib.Imaging.PixelConversion;

namespace CommonLib.Utils {
    public static class BitmapUtils {
        public static byte[] ConvertABGR1555DataToARGB1555BitmapData(ushort[,] imageData) {
            var imageDataBytes = new byte[imageData.GetLength(0) * imageData.GetLength(1) * 2];
            var pos = 0;
            for (var y = 0; y < imageData.GetLength(1); y++) {
                for (var x = 0; x < imageData.GetLength(0); x++) {
                    var newBits = ABGR1555toARGB8888(imageData[x, y]);
                    imageDataBytes[pos++] = (byte) ((newBits >> 0) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >> 8) & 0xFF);
                }
            }
            return imageDataBytes;
        }

        public static byte[] ConvertABGR1555DataToARGB8888BitmapData(ushort[,] imageData) {
            var imageDataBytes = new byte[imageData.GetLength(0) * imageData.GetLength(1) * 4];
            var pos = 0;
            for (var y = 0; y < imageData.GetLength(1); y++) {
                for (var x = 0; x < imageData.GetLength(0); x++) {
                    var newBits = ABGR1555toARGB8888(imageData[x, y]);
                    imageDataBytes[pos++] = (byte) ((newBits >>  0) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >>  8) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >> 16) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >> 24) & 0xFF);
                }
            }
            return imageDataBytes;
        }

        public static byte[] ConvertIndexedDataToARGB1555BitmapData(byte[,] imageData, Palette palette, bool zeroIsTransparent) {
            var imageDataBytes = new byte[imageData.GetLength(0) * imageData.GetLength(1) * 2];
            var pos = 0;
            for (var y = 0; y < imageData.GetLength(1); y++) {
                for (var x = 0; x < imageData.GetLength(0); x++) {
                    var newBits = IndexedToARGB1555(imageData[x, y], palette, zeroIsTransparent);
                    imageDataBytes[pos++] = (byte) ((newBits >> 0) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >> 8) & 0xFF);
                }
            }
            return imageDataBytes;
        }

        public static byte[] ConvertIndexedDataToARGB8888BitmapData(byte[,] imageData, Palette palette, bool zeroIsTransparent) {
            var imageDataBytes = new byte[imageData.GetLength(0) * imageData.GetLength(1) * 4];
            var pos = 0;
            for (var y = 0; y < imageData.GetLength(1); y++) {
                for (var x = 0; x < imageData.GetLength(0); x++) {
                    var newBits = IndexedToARGB8888(imageData[x, y], palette, zeroIsTransparent);
                    imageDataBytes[pos++] = (byte) ((newBits >>  0) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >>  8) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >> 16) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >> 24) & 0xFF);
                }
            }
            return imageDataBytes;
        }
    }
}
