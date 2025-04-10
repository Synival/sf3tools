using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Imaging;
using static CommonLib.Imaging.PixelConversion;

namespace SF3.Win.Extensions {
    public static class BitmapExtensions {
        /// <summary>
        /// Draws a bitmap onto another bitmap without using Graphics.FromImage(), which can throw 'OutOfMemoryException'
        /// due to GDI+ implementation bugs.
        /// </summary>
        /// <param name="to">The bitmap to copy an image to.</param>
        /// <param name="from">The bitmap to copy to an image.</param>
        /// <param name="x">X coordinate of 'to' to copy to.</param>
        /// <param name="y">Y coordinate of 'to' to copy to.</param>
        public static void SafeDrawImage(this Bitmap to, Bitmap from, int x, int y) {
            for (var iy = Math.Max(-y, 0); iy < from.Height && iy + y < to.Height; iy++)
                for (var ix = Math.Max(-x, 0); ix < from.Width && ix + x < to.Width; ix++)
                    to.SetPixel(ix + x, iy + y, from.GetPixel(ix, iy));
        }

        public static byte[] GetDataBGRA8888(this Bitmap bitmap) {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                throw new ArgumentException(nameof(bitmap.PixelFormat));

            var readBytes = new byte[bitmap.Width * bitmap.Height * 4];
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            Marshal.Copy(bitmapData.Scan0, readBytes, 0, readBytes.Length);
            bitmap.UnlockBits(bitmapData);

            return readBytes;
        }

        public static ushort[,] Get2DDataABGR1555(this Bitmap bitmap) {
            var outputData = new ushort[bitmap.Width, bitmap.Height];
            var inputData = bitmap.GetDataBGRA8888();

            int pos = 0;
            for (var y = 0; y < bitmap.Height; y++) {
                for (var x = 0; x < bitmap.Width; x++) {
                    var channels = new PixelChannels() {
                        b = inputData[pos++],
                        g = inputData[pos++],
                        r = inputData[pos++],
                        a = inputData[pos++]
                    };
                    outputData[x, y] = channels.ToABGR1555();
                }
            }

            return outputData;
        }

        public static TextureABGR1555 CreateTextureABGR1555(this Bitmap bitmap, int id, int frame, int duration)
            => new TextureABGR1555(id, frame, duration, Get2DDataABGR1555(bitmap));
    }
}
