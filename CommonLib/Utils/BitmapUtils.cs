namespace CommonLib.Utils {
    public static class BitmapUtils {
        public static byte[] ConvertABGR1555DataToABGR1555BitmapData(ushort[,] imageData) {
            var imageDataBytes = new byte[imageData.GetLength(0) * imageData.GetLength(1) * 2];
            var pos = 0;
            for (var y = 0; y < imageData.GetLength(1); y++) {
                for (var x = 0; x < imageData.GetLength(0); x++) {
                    var newBits = PixelConversion.ARGB1555toABGR1555(imageData[x, y]);
                    imageDataBytes[pos++] = (byte) (newBits & 0x00FF);
                    imageDataBytes[pos++] = (byte) ((newBits & 0xFF00) >> 8);
                }
            }
            return imageDataBytes;
        }
    }
}
