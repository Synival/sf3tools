using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Imaging;
using static CommonLib.Imaging.PixelConversion;

namespace CommonLib.Utils {
    public static class BitmapUtils {
        public static unsafe byte[] ConvertABGR1555DataToARGB1555BitmapData(ushort[,] input, bool highlightEndcodes = false) {
            if (input == null)
                return null;

            var output = new byte[input.GetLength(0) * input.GetLength(1) * 2];
            var pos = 0;

            fixed (byte* pOutput = output) {
                if (highlightEndcodes) {
                    for (var y = 0; y < input.GetLength(1); y++) {
                        for (var x = 0; x < input.GetLength(0); x++) {
                            var newBits = ABGR1555toARGB1555(input[x, y]);
                            if (newBits == 0x7FFF)
                                newBits = 0xFC1F;
                            pOutput[pos++] = (byte) ((newBits >> 0) & 0xFF);
                            pOutput[pos++] = (byte) ((newBits >> 8) & 0xFF);
                        }
                    }
                }
                else {
                    for (var y = 0; y < input.GetLength(1); y++) {
                        for (var x = 0; x < input.GetLength(0); x++) {
                            var newBits = ABGR1555toARGB1555(input[x, y]);
                            pOutput[pos++] = (byte) ((newBits >> 0) & 0xFF);
                            pOutput[pos++] = (byte) ((newBits >> 8) & 0xFF);
                        }
                    }
                }
            }

            return output;
        }

        public static unsafe byte[] ConvertABGR1555DataToARGB8888BitmapData(ushort[,] imageData, bool highlightEndcodes = false) {
            if (imageData == null)
                return null;

            var output = new byte[imageData.GetLength(0) * imageData.GetLength(1) * 4];
            var pos = 0;

            fixed (byte* pOutput = output) {
                if (highlightEndcodes) {
                    for (var y = 0; y < imageData.GetLength(1); y++) {
                        for (var x = 0; x < imageData.GetLength(0); x++) {
                            var newBits = ABGR1555toARGB8888(imageData[x, y]);
                            if (newBits == 0xFF000000)
                                newBits = 0xFFFF00FF;

                            pOutput[pos++] = (byte) ((newBits >>  0) & 0xFF);
                            pOutput[pos++] = (byte) ((newBits >>  8) & 0xFF);
                            pOutput[pos++] = (byte) ((newBits >> 16) & 0xFF);
                            pOutput[pos++] = (byte) ((newBits >> 24) & 0xFF);
                        }
                    }
                }
                else {
                    for (var y = 0; y < imageData.GetLength(1); y++) {
                        for (var x = 0; x < imageData.GetLength(0); x++) {
                            var newBits = ABGR1555toARGB8888(imageData[x, y]);
                            pOutput[pos++] = (byte) ((newBits >>  0) & 0xFF);
                            pOutput[pos++] = (byte) ((newBits >>  8) & 0xFF);
                            pOutput[pos++] = (byte) ((newBits >> 16) & 0xFF);
                            pOutput[pos++] = (byte) ((newBits >> 24) & 0xFF);
                        }
                    }
                }
            }

            return output;
        }

        public static unsafe byte[] ConvertIndexedDataToARGB1555BitmapData(byte[] input, IPalette palette, bool zeroIsTransparent) {
            if (input == null)
                return null;

            var output = new byte[input.Length * 2];
            var inputLen = input.Length;

            fixed (byte* pInput = input, pOutput = output) {
                int pos = 0;
                for (var i = 0; i < inputLen; i++) {
                    var newBits = IndexedToARGB1555(pInput[i], palette, zeroIsTransparent);
                    pOutput[pos++] = (byte) ((newBits >> 0) & 0xFF);
                    pOutput[pos++] = (byte) ((newBits >> 8) & 0xFF);
                }
            }

            return output;
        }

        public static unsafe byte[] ConvertIndexedDataToARGB1555BitmapData(byte[,] input, IPalette palette, bool zeroIsTransparent) {
            if (input == null)
                return null;

            var output = new byte[input.GetLength(0) * input.GetLength(1) * 2];
            var pos = 0;

            fixed (byte* pOutput = output) {
                for (var y = 0; y < input.GetLength(1); y++) {
                    for (var x = 0; x < input.GetLength(0); x++) {
                        var newBits = IndexedToARGB1555(input[x, y], palette, zeroIsTransparent);
                        pOutput[pos++] = (byte) ((newBits >> 0) & 0xFF);
                        pOutput[pos++] = (byte) ((newBits >> 8) & 0xFF);
                    }
                }
            }

            return output;
        }

        public static unsafe byte[] ConvertIndexedDataToARGB8888BitmapData(byte[] input, IPalette palette, bool zeroIsTransparent) {
            if (input == null)
                return null;

            var output = new byte[input.Length * 4];
            var inputLen = input.Length;

            fixed (byte* pInput = input, pOutput = output) {
                int pos = 0;
                for (var i = 0; i < inputLen; i++) {
                    var newBits = IndexedToARGB8888(pInput[i], palette, zeroIsTransparent);
                    pOutput[pos++] = (byte) ((newBits >>  0) & 0xFF);
                    pOutput[pos++] = (byte) ((newBits >>  8) & 0xFF);
                    pOutput[pos++] = (byte) ((newBits >> 16) & 0xFF);
                    pOutput[pos++] = (byte) ((newBits >> 24) & 0xFF);
                }
            }

            return output;
        }

        public static unsafe byte[] ConvertIndexedDataToARGB8888BitmapData(byte[,] input, IPalette palette, bool zeroIsTransparent) {
            if (input == null)
                return null;

            var output = new byte[input.GetLength(0) * input.GetLength(1) * 4];
            var pos = 0;

            fixed (byte* pOutput = output) {
                for (var y = 0; y < input.GetLength(1); y++) {
                    for (var x = 0; x < input.GetLength(0); x++) {
                        var newBits = IndexedToARGB8888(input[x, y], palette, zeroIsTransparent);
                        pOutput[pos++] = (byte) ((newBits >>  0) & 0xFF);
                        pOutput[pos++] = (byte) ((newBits >>  8) & 0xFF);
                        pOutput[pos++] = (byte) ((newBits >> 16) & 0xFF);
                        pOutput[pos++] = (byte) ((newBits >> 24) & 0xFF);
                    }
                }
            }

            return output;
        }

        public static void SaveBitmapToFile(string filename, ushort[,] imageData, ImageFormat imageFormat) {
            var width  = imageData.GetLength(0);
            var height = imageData.GetLength(1);
            var imageByteData = ConvertABGR1555DataToARGB1555BitmapData(imageData);

            using (var bitmap = new Bitmap(width, height, PixelFormat.Format16bppArgb1555)) {
                var bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                Marshal.Copy(imageByteData, 0, bitmapData.Scan0, imageByteData.Length);
                bitmap.UnlockBits(bitmapData);
                bitmap.Save(filename, imageFormat);
            }
        }
    }
}
