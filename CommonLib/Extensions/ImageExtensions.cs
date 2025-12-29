using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Imaging;
using CommonLib.Utils;

namespace CommonLib.Extensions {
    public static class ImageExtensions {
        public static Bitmap CreateIndexedBitmap(this Image image) {
            if (image.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new ArgumentException($"Bitmap pixel format ({image.PixelFormat}) should be 'Format8bppIndexed'");

            var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format8bppIndexed);
            bitmap.SetPalette(image.GetPalette());

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
            Marshal.Copy(image.GetDataIndexed(), 0, bitmapData.Scan0, bitmap.Width * bitmap.Height);
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }

        public static Bitmap CreateARGB8888Bitmap(this Image image, bool zeroIsTransparent = false) {
            var bitmap = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);

            // Try to get raw data if possible.
            byte[] newData = null;
            if (image is Bitmap imageAsBitmap) {
                switch (imageAsBitmap.PixelFormat) {
                    case PixelFormat.Format8bppIndexed:
                        newData = BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(imageAsBitmap.GetBitmapDataIndexed(), imageAsBitmap.GetPalette(), zeroIsTransparent);
                        break;
                }
            }

            // We found data -- copy it in.
            if (newData != null) {
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                Marshal.Copy(newData, 0, bitmapData.Scan0, bitmap.Width * bitmap.Height * 4);
                bitmap.UnlockBits(bitmapData);
            }
            // As a fallback, use Graphics.DrawImage().
            else {
                using (var graphics = Graphics.FromImage(bitmap)) {
                    graphics.DrawImage(image, 0, 0);
                    graphics.Flush();
                }
            }

            return bitmap;
        }

        public static byte[] GetBitmapDataBGRA8888(this Image image, bool zeroIsTransparent = false) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.GetBitmapDataBGRA8888(bitmap, zeroIsTransparent);
            else {
                using (bitmap = image.CreateARGB8888Bitmap())
                    return BitmapExtensions.GetBitmapDataBGRA8888(bitmap, zeroIsTransparent);
            }
        }

        public static byte[] GetDataIndexed(this Image image) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.GetDataIndexed(bitmap);
            else {
                using (bitmap = image.CreateIndexedBitmap())
                    return BitmapExtensions.GetDataIndexed(bitmap);
            }
        }

        public static byte[,] Get2DDataIndexed(this Image image) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.Get2DDataIndexed(bitmap);
            else {
                using (bitmap = image.CreateIndexedBitmap())
                    return BitmapExtensions.Get2DDataIndexed(bitmap);
            }
        }

        public static ushort[] GetDataABGR1555(this Image image, bool zeroIsTransparent = false) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.GetDataABGR1555(bitmap, zeroIsTransparent);
            else {
                using (bitmap = image.CreateARGB8888Bitmap(zeroIsTransparent))
                    return BitmapExtensions.GetDataABGR1555(bitmap, zeroIsTransparent);
            }
        }

        public static ushort[,] Get2DDataABGR1555(this Image image, bool zeroIsTransparent = false) {
            if (image is Bitmap bitmap)
                return BitmapExtensions.Get2DDataABGR1555(bitmap, zeroIsTransparent);
            else {
                using (bitmap = image.CreateARGB8888Bitmap(zeroIsTransparent))
                    return BitmapExtensions.Get2DDataABGR1555(bitmap, zeroIsTransparent);
            }
        }

        public static void SetPalette(this Image bitmap, Palette palette) {
            var outputPalette = bitmap.Palette;
            var palLen = Math.Min(256, palette.Channels.Length);

            for (int i = 0; i < palLen; ++i) {
                var inputColor = palette[i];
                outputPalette.Entries[i] = Color.FromArgb(inputColor.r, inputColor.g, inputColor.b);
            }
            bitmap.Palette = outputPalette;
        }

        public static Palette GetPalette(this Image bitmap) {
            var inputPalette = bitmap.Palette;
            var outputColors = new ushort[inputPalette.Entries.Length];

            for (int i = 0; i < outputColors.Length; ++i) {
                var inputColor = inputPalette.Entries[i];
                outputColors[i] = new PixelChannels { r = inputColor.R, g = inputColor.G, b = inputColor.B, a = inputColor.A }.ToABGR1555();
            }

            return new Palette(outputColors);
        }
    }
}
