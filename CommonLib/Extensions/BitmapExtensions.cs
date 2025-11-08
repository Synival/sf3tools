using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Imaging;

namespace CommonLib.Extensions {
    public static class BitmapExtensions {
        /// <summary>
        /// Gets image data in ABGR1555 format for a subsection.
        /// </summary>
        /// <param name="bitmap">Bitmap to get image data from.</param>
        /// <param name="x">Topleft X coordinate of subsection.</param>
        /// <param name="y">Topleft Y coordinate of subsection.</param>
        /// <param name="width">Width of subsection.</param>
        /// <param name="height">Height of subsection.</param>
        /// <returns>A 2D array[width, height] of ushorts representting colors in ABGR1555 format.</returns>
        public static ushort[,] GetDataAt(this Bitmap bitmap, int x, int y, int width, int height) {
            // Only 16- or 32-bit bitmaps are currently supported.
            if (bitmap == null || !(bitmap.PixelFormat == PixelFormat.Format16bppArgb1555 || bitmap.PixelFormat == PixelFormat.Format32bppArgb))
                return null;

            // Image must be within bounds.
            var x2 = x + width;
            var y2 = y + height;
            if (x < 0 || y < 0 || x2 > bitmap.Width || y2 > bitmap.Height)
                return null;

            // Looks like we should be able to get a sub-image. Get the image data.
            var data = new ushort[width * height];
            var bytesPerPixel = (bitmap.PixelFormat == PixelFormat.Format32bppArgb) ? 4 : 2;
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            unsafe {
                var bitmapDataPtr = (byte*) bitmapData.Scan0.ToPointer();
                int writePos = 0;
                for (var iy = y; iy < y2; iy++) {
                    var readPos = (iy * bitmap.Width + x) * bytesPerPixel;
                    for (var ix = x; ix < x2; ix++) {
                        uint bitmapColor = 0;
                        for (int i = 0; i < bytesPerPixel; i++)
                            bitmapColor |= (uint) (bitmapDataPtr[readPos++] << (i * 8));
                        data[writePos++] = (bytesPerPixel == 4)
                            ? PixelConversion.ARGB8888toABGR1555(bitmapColor)
                            : PixelConversion.ARGB1555toABGR1555((ushort) bitmapColor);
                    }
                }
            }
            bitmap.UnlockBits(bitmapData);

            // Return as a 2D array with [x][y] accessors.
            return data.To2DArrayColumnMajor(width, height);
        }

        /// <summary>
        /// Sets a subsection of image data in ABGR1555 format.
        /// </summary>
        /// <param name="bitmap">Bitmap to set modify.</param>
        /// <param name="x">Topleft X coordinate of subsection.</param>
        /// <param name="y">Topleft Y coordinate of subsection.</param>
        /// <param name="data">2D array[width, height] of color data in ABGR1555 format.</param>
        /// <returns>Returns 'true' if the subsection was written, otherwise 'false'.</returns>
        public static bool SetDataAt(this Bitmap bitmap, int x, int y, ushort[,] data) {
            // Only 16- or 32-bit bitmaps are currently supported.
            if (bitmap == null || !(bitmap.PixelFormat == PixelFormat.Format16bppArgb1555 || bitmap.PixelFormat == PixelFormat.Format32bppArgb))
                return false;

            // Image must be within bounds.
            var width = data.GetLength(0);
            var height = data.GetLength(1);
            var x2 = x + width;
            var y2 = y + height;
            if (x < 0 || y < 0 || x2 > bitmap.Width || y2 > bitmap.Height)
                return false;

            // Looks like we should be able to set a sub-image. Get the image data.
            var bytesPerPixel = (bitmap.PixelFormat == PixelFormat.Format32bppArgb) ? 4 : 2;
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

            unsafe {
                var bitmapDataPtr = (byte*) bitmapData.Scan0.ToPointer();
                int readY = 0;
                for (var iy = y; iy < y2; iy++, readY++) {
                    var writePos = (iy * bitmap.Width + x) * bytesPerPixel;

                    int readX = 0;
                    for (var ix = x; ix < x2; ix++, readX++) {
                        var colorABGR1555 = data[readX, readY];
                        var bitmapColor = (bytesPerPixel == 4)
                            ? PixelConversion.ABGR1555toARGB8888(colorABGR1555)
                            : PixelConversion.ABGR1555toARGB1555(colorABGR1555);

                        for (int i = 0; i < bytesPerPixel; i++)
                            bitmapDataPtr[writePos++] = (byte) (bitmapColor >> (i * 8));
                    }
                }
            }
            bitmap.UnlockBits(bitmapData);

            // Success!
            return true;
        }

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

        public static byte[] GetBitmapDataBGRA8888(this Bitmap bitmap) {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
                using (bitmap = bitmap.CreateARGB8888Bitmap())
                    return GetBitmapDataBGRA8888(bitmap);

            var readBytes = new byte[bitmap.Width * bitmap.Height * 4];
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
            Marshal.Copy(bitmapData.Scan0, readBytes, 0, readBytes.Length);
            bitmap.UnlockBits(bitmapData);

            return readBytes;
        }

        public static ushort[] GetDataABGR1555(this Bitmap bitmap) {
            var outputData = new ushort[bitmap.Width * bitmap.Height];
            var inputData = bitmap.GetBitmapDataBGRA8888();

            int pos = 0, outPos = 0;
            for (var y = 0; y < bitmap.Height; y++) {
                for (var x = 0; x < bitmap.Width; x++) {
                    var channels = new PixelChannels() {
                        b = inputData[pos++],
                        g = inputData[pos++],
                        r = inputData[pos++],
                        a = inputData[pos++]
                    };
                    outputData[outPos++] = channels.ToABGR1555();
                }
            }

            return outputData;
        }

        public static ushort[,] Get2DDataABGR1555(this Bitmap bitmap) {
            var outputData = new ushort[bitmap.Width, bitmap.Height];
            var inputData = bitmap.GetBitmapDataBGRA8888();

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

        public static void UsePalette(this Bitmap bitmap, Palette palette) {
            var outputPalette = bitmap.Palette;
            var palLen = Math.Min(256, palette.Channels.Length);

            for (int i = 0; i < palLen; ++i) {
                var inputColor = palette[i];
                outputPalette.Entries[i] = Color.FromArgb(inputColor.r, inputColor.g, inputColor.b);
            }
            bitmap.Palette = outputPalette;
        }
    }
}
