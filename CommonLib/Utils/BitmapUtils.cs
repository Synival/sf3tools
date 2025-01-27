namespace CommonLib.Utils {
    public static class BitmapUtils {
        public static byte[] ConvertABGR1555DataToABGR1555BitmapData(ushort[,] imageData) {
            var imageDataBytes = new byte[imageData.GetLength(0) * imageData.GetLength(1) * 2];
            var pos = 0;
            for (var y = 0; y < imageData.GetLength(1); y++) {
                for (var x = 0; x < imageData.GetLength(0); x++) {
                    var newBits = PixelConversion.ARGB1555toABGR1555(imageData[x, y]);
                    imageDataBytes[pos++] = (byte) ((newBits >> 0) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >> 8) & 0xFF);
                }
            }
            return imageDataBytes;
        }

        public static byte[] ConvertABGR1555DataToABGR8888BitmapData(ushort[,] imageData) {
            var imageDataBytes = new byte[imageData.GetLength(0) * imageData.GetLength(1) * 4];
            var pos = 0;
            for (var y = 0; y < imageData.GetLength(1); y++) {
                for (var x = 0; x < imageData.GetLength(0); x++) {
                    var newBits = PixelConversion.ARGB1555toABGR8888(imageData[x, y]);
                    imageDataBytes[pos++] = (byte) ((newBits >>  0) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >>  8) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >> 16) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >> 24) & 0xFF);
                }
            }
            return imageDataBytes;
        }

        public static byte[] ConvertIndexedDataToABGR1555BitmapData(byte[,] imageData) {
            var imageDataBytes = new byte[imageData.GetLength(0) * imageData.GetLength(1) * 2];
            var pos = 0;
            for (var y = 0; y < imageData.GetLength(1); y++) {
                for (var x = 0; x < imageData.GetLength(0); x++) {
                    var newBits = PixelConversion.IndexedToABGR1555(imageData[x, y]);
                    imageDataBytes[pos++] = (byte) ((newBits >> 0) & 0xFF);
                    imageDataBytes[pos++] = (byte) ((newBits >> 8) & 0xFF);
                }
            }
            return imageDataBytes;
        }

        public static byte[] ConvertIndexedDataToABGR8888BitmapData(byte[,] imageData) {
            var imageDataBytes = new byte[imageData.GetLength(0) * imageData.GetLength(1) * 4];
            var pos = 0;
            for (var y = 0; y < imageData.GetLength(1); y++) {
                for (var x = 0; x < imageData.GetLength(0); x++) {
                    var newBits = PixelConversion.IndexedToABGR8888(imageData[x, y]);
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
